using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UploadingFiles.Web.Binders
{
    public class JsonNetModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
                                             PropertyDescriptor propertyDescriptor)
        {
            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                //According to the Content type, only json.net this content type will use the ModelBinder, or use the default Binder
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);

                return;
            }

            // need to skip properties that aren't part of the request, else we might hit a StackOverflowException
            string name = propertyDescriptor.Name;

            foreach (object attribute in propertyDescriptor.Attributes)
            {
                if (attribute is JsonPropertyAttribute)
                {
                    var jsonProperty = attribute as JsonPropertyAttribute;
                    name = jsonProperty.PropertyName;
                }
            }

            string fullPropertyKey = CreateSubPropertyName(bindingContext.ModelName, name);

            if (!bindingContext.ValueProvider.ContainsPrefix(fullPropertyKey))
            {
                return;
            }

            // call into the property's model binder
            IModelBinder propertyBinder = Binders.GetBinder(propertyDescriptor.PropertyType);
            object originalPropertyValue = propertyDescriptor.GetValue(bindingContext.Model);
            ModelMetadata propertyMetadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];
            propertyMetadata.Model = originalPropertyValue;

            var innerBindingContext = new ModelBindingContext
            {
                ModelMetadata = propertyMetadata,
                ModelName = fullPropertyKey,
                ModelState = bindingContext.ModelState,
                ValueProvider = bindingContext.ValueProvider
            };

            object newPropertyValue = GetPropertyValue(controllerContext, innerBindingContext, propertyDescriptor,
                                                       propertyBinder);
            propertyMetadata.Model = newPropertyValue;

            // validation
            ModelState modelState = bindingContext.ModelState[fullPropertyKey];

            if (modelState == null || modelState.Errors.Count == 0)
            {
                if (OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, newPropertyValue))
                {
                    SetProperty(controllerContext, bindingContext, propertyDescriptor, newPropertyValue);
                    OnPropertyValidated(controllerContext, bindingContext, propertyDescriptor, newPropertyValue);
                }
            }
            else
            {
                SetProperty(controllerContext, bindingContext, propertyDescriptor, newPropertyValue);

                // Convert FormatExceptions (type conversion failures) into InvalidValue messages
                foreach (ModelError error in modelState.Errors
                    .Where(err => String.IsNullOrEmpty(err.ErrorMessage) && err.Exception != null)
                    .ToList())
                {
                    for (Exception exception = error.Exception; exception != null; exception = exception.InnerException)
                    {
                        if (exception is FormatException)
                        {
                            string displayName = propertyMetadata.GetDisplayName();
                            string errorMessageTemplate = "The value '{0}' is not valid for {1}.";
                            string errorMessage = String.Format(CultureInfo.CurrentCulture, errorMessageTemplate,
                                                                modelState.Value.AttemptedValue, displayName);
                            modelState.Errors.Remove(error);
                            modelState.Errors.Add(errorMessage);

                            break;
                        }
                    }
                }
            }
        }
    }
}

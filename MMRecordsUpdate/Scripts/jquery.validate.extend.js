jQuery.validator.addMethod("mustbetrue", function (value, element, param) {
    return element.checked;
});

jQuery.validator.unobtrusive.adapters.addBool("mustbetrue");

jQuery.validator.addMethod("mustbefalse", function (value, element, param) {
    return !element.checked;
});

jQuery.validator.unobtrusive.adapters.addBool("mustbefalse");

jQuery.validator.addMethod("requiredif", function (value, element, params) {
    var actualValue = $('[name="' + params.propertyname + '"]').val();
    var desiredValues = params.desiredvalue.toLowerCase().split(',');
    var controlType = params.controltype.toLowerCase();
    var matched = false;

    if (controlType == "radio") {
        actualValue = $('[name="' + params.propertyname + '"]:checked').val();        
    }

    if (actualValue === undefined || actualValue == null) {
        actualValue = "";
    }

    for (var i = 0; i < desiredValues.length; i++) {
        if (actualValue.toLowerCase() == desiredValues[i].toLowerCase()) {
            matched = true;
            break;
        }
    }

    //actualValue.toLowerCase() == params.desiredvalue.toLowerCase()
    if (matched && !value) {
        return false;
    }
    return true;
});

jQuery.validator.unobtrusive.adapters.add('requiredif', ['propertyname', 'desiredvalue', 'controltype'],
    function (options) {
        options.rules['requiredif'] = options.params;
        options.messages['requiredif'] = options.message;
    });

jQuery.validator.addMethod("requiredifnot", function (value, element, params) {
    var actualValue = $('[name="' + params.propertyname + '"]').val();
    if (actualValue === undefined || actualValue == null) {
        actualValue = "";
    }

    if (actualValue.toLowerCase() != params.desiredvalue.toLowerCase() && !value) {
        return false;
    }
    return true;
});

jQuery.validator.unobtrusive.adapters.add('requiredifnot', ['propertyname', 'desiredvalue'],
    function (options) {
        options.rules['requiredifnot'] = options.params;
        options.messages['requiredifnot'] = options.message;
    });

// file upload validator
jQuery.validator.addMethod("uploadvalidator", function (value, element, params) {
    if (value.length === 0) {
        return true;
    }
    return (new RegExp('(' + params.extensions + ')$')).test(value);
});

jQuery.validator.unobtrusive.adapters.add('uploadvalidator', ['extensions'],
    function (options) {
        options.rules['uploadvalidator'] = options.params;
        options.messages['uploadvalidator'] = options.message;
    });
;

// checkboxlist validator
// NOTE: $("form").data("validator").settings.ignore = ""; on page to allow validation of a hidden field
jQuery.validator.addMethod("checkboxlistvalidator", function (value, element, params) {
    
    var cbClass = $(element).data("val-class"),
        checked = $("." + cbClass + ":checked").length;

    console.log(checked >= params.minimum);

    return checked >= params.minimum;
});

jQuery.validator.unobtrusive.adapters.add('checkboxlistvalidator', ['minimum'], function (options) {
    options.rules['checkboxlistvalidator'] = options.params;
    options.messages['checkboxlistvalidator'] = options.message;
});
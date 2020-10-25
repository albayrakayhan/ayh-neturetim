var customValidation = {};

customValidation.siparis = function () {
    $("#siparisKart").validate({
        rules: {
            stok_kodu: "required",
            musteri_carikart_id : "required",
            zorlukgrubu_id :"required",
            siparisturu_id : "required",
            siparis_tarihi : "required"
        },
        messages: {
            stok_kodu: "Lütfen stok kodu girin!",
            musteri_carikart_id : "Lütfen müşteri seçiniz.",
            zorlukgrubu_id : "Lütffen zorluk grubu seçiniz.",
            siparisturu_id : "Lütfen sipariş türü seçiniz.",
            siparis_tarihi : "Lütfen sipariş tarihini girin"
        },

        errorElement: "em",
        errorPlacement: function (error, element) {
            // Add the `help-block` class to the error element
            error.addClass("help-block");

            if (element.prop("type") === "checkbox") {
                error.insertAfter(element.parent("label"));
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).parents(".input-area").addClass("has-error").removeClass("has-success");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents(".input-area").addClass("has-success").removeClass("has-error");
        }
    });
}

customValidation.modelKart = function () {
    $("#modelKart").validate({
        rules: {
            stok_kodu: "required",
            stok_adi: "required",
            stok_adi_uzun: {
                required : true,
                minlength: 2,
                /*normalizer : function(value){
                    return $.trim( value );
                }
                */
            },
            stokkart_tipi_id: "required",
        },
        messages: {
            stok_kodu: "Lütfen stok kodu girin!",
            stok_adi: "Lütfen kısa ad girin!",
            stok_adi_uzun: "Lütfen uzun ad girin!",
            stokkart_tipi_id: "Lütfen stokkart tipi seçin!",
        },
        errorElement: "em",
        errorPlacement: function (error, element) {
            // Add the `help-block` class to the error element
            error.addClass("help-block");

            if (element.prop("type") === "checkbox") {
                error.insertAfter(element.parent("label"));
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).parents(".input-area").addClass("has-error").removeClass("has-success");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents(".input-area").addClass("has-success").removeClass("has-error");
        }
    });
}

customValidation.modelKartTalimatlar = function () {
     $("#talimatForm").validate({
        rules: {
            sira_id: "required",
            talimatturu_id: "required",
            aciklama: "required",
            //irstalimat: "required",
        },
        messages: {
            sira_id: "Zorunlu!",
            talimatturu_id: "Zorunlu!",
            aciklama: "Zorunlu!",
            //irstalimat: "Zorunlu!",
        },
        errorElement: "em",
        errorPlacement: function (error, element) {
            // Add the `help-block` class to the error element
            error.addClass("help-block");

            if (element.prop("type") === "checkbox") {
                error.insertAfter(element.parent("label"));
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).parents(".input-area").addClass("has-error").removeClass("has-success");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents(".input-area").addClass("has-success").removeClass("has-error");
        }
    });
}

customValidation.eklerFrom = function () {
    $("#eklerFrom").validate({
        rules: {
            ekturu_id: "required",
            dosyaSec: "required"
        },
        messages: {
            ekturu_id: "Zorunlu!",
            dosyaSec: "Lütfen dosya seçin!"
        },
        errorElement: "em",
        errorPlacement: function (error, element) {
            // Add the `help-block` class to the error element
            error.addClass("help-block");

            if (element.prop("type") === "checkbox") {
                error.insertAfter(element.parent("label"));
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).parents(".input-area").addClass("has-error").removeClass("has-success");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents(".input-area").addClass("has-success").removeClass("has-error");
        }
    });
}

customValidation.generic_form_validation = function () {
    $("#newFrom").validate({
        errorElement: "em",
        errorPlacement: function (error, element) {
            // Add the `help-block` class to the error element
            error.addClass("help-block");

            if (element.prop("type") === "checkbox") {
                error.insertAfter(element.parent("label"));
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).parents(".input-area").addClass("has-error").removeClass("has-success");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents(".input-area").addClass("has-success").removeClass("has-error");
        }
    });
}

customValidation.stokKart = function () {
    $("#stokKart").validate({
        rules: {
            stok_kodu: "required",
            stok_adi: "required",
            stok_adi_uzun: "required",
            stokkart_tipi_id: "required",
        },
        messages: {
            stok_kodu: "Lütfen stok kodu girin!",
            stok_adi: "Lütfen kısa ad girin!",
            stok_adi_uzun: "Lütfen uzun ad girin!",
            stokkart_tipi_id: "Lütfen stokkart tipi seçin!",
        },
        errorElement: "em",
        errorPlacement: function (error, element) {
            // Add the `help-block` class to the error element
            error.addClass("help-block");

            if (element.prop("type") === "checkbox") {
                error.insertAfter(element.parent("label"));
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).parents(".input-area").addClass("has-error").removeClass("has-success");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents(".input-area").addClass("has-success").removeClass("has-error");
        }
    });
}


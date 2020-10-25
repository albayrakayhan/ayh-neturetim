var AjaxHelper = {

    data: null,
    cal: function (options) {

        let defaults = {
            type: "GET",
            url: "",
            async: true,
            dataType: "json",
            apiBaseUrl : apiUrl,
        }

        var settings = $.extend({}, defaults, options);

        return $.ajax({
            type: settings.method,
            url: url,
            async: settings.async,
            dataType: settings.dataType,
            success: settings.success,
        });
    }
}

var netUretimApi = {

    parametre: {
        
        getOlcuBirimList: function () {

            let metodUrl = 'api/admin/birimliste';
            let storageKey = "OlcuBirimList";

            let responseData = localStorage.getItem(storageKey);

            if (responseData !== null) {
                return JSON.parse(responseData)
            }

            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                async: true,
                dataType: "json",
                success: function (data) {
                    localStorage.setItem(storageKey, JSON.stringify(data))
                },
                error: function () {
                    console.log("hata oluştu");
                }
            });

            return JSON.parse(localStorage.getItem(storageKey));
        }
    }
};
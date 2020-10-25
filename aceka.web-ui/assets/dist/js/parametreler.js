var apiUrl = 'http://92.45.23.86:9597/';

parametre.parametreler = {

    CarikartTurleri: function (controlId) {
        var metodUrl = 'api/parametre/cari-kart-turleri ';

        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            success: function (data) {
                if (data != null) {

                    $('#' + controlId + ' option').remove();
                    $('#' + controlId).append('<option value="">Cari Kart Türü Seçin</option>');

                    $.each(data, function (key, obj) {

                        $('#' + controlId).append('<option value="' + obj.ulke_id + '">' + obj.ulke_adi + '</option>');
                    });
                }
                else {
                    $('#' + controlId + ' option').remove();
                    $('#' + controlId).append('<option value="">Cari Kart Türü Seçin</option>');
                }
            }
        });
    }
};
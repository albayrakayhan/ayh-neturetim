$(document).ready(function () {
    /*stokKodu*/
    //$(function () {
    //    var availableTags = [
    //      "Ada Plastik",
    //      "Ara Nv",
    //      "Avery",
    //      "Bogaz içi tesktil",
    //    ];
    //    $("#satici_carikart_id").autocomplete({
    //        source: availableTags
    //    });
    //});
    /*stokKodu*/

    /*checkbox*/
    checkbox();
    /*checkbox*/

    /*Genel image tablet elemenets*/
    $(function () {
        $('#imageDownload').DataTable({
            "paging": true,
            "lengthChange": false,
            "searching": false,
            "ordering": true,
            "info": true,
            "autoWidth": true,
            "pageLength": 5,
        });
    });
    /*Genel image tablet elemenets*/

    /*tr delete*/
    $('.delete').click(function () {
        var checked = $('input[type="checkbox"]:checked').parent().parent().parent().remove();
    })
    /*tr delete*/

    /*ilkMaddeKumas tablet elemenets*/
    $(function () {
        $('.tableDesing').DataTable({
            "paging": true,
            "lengthChange": false,
            "searching": false,
            "ordering": true,
            "info": true,
            "autoWidth": true,
            "pageLength": 5,
        });
    });
    /*ilkMaddeKumas tablet elemenets*/

    /*Genel imageDownload tr click popup*/
    $('#imageDownload tbody tr').dblclick(function () {
        var elem = $(this);
        var text = elem.find('td').first().text();
        $('.imageItem a img').attr('src', text);
    })
    /*Genel imageDownload tr click popup*/

    /*Genel tr click image*/
    $('#varyantlar tbody tr').dblclick(function () {
        var elem = $(this);
        $('#modalPopup').modal('show')
    })
    /*Genel tr click image*/

    /*ilk Madde Kumas tr click popup*/
    $('#ilkMaddeKumas tbody tr').dblclick(function () {
        var elem = $(this);
        $('#modalPopup').modal('show')
    })
    /*ilk Madde Kumas tr click popup*/

    /*İlk Madde Aksesuar tr click popup*/
    $('#İlkMaddeAksesuar tbody tr').dblclick(function () {
        var elem = $(this);
        $('#modalPopup').modal('show')
    })
    /*İlk Madde Aksesuar tr click popup*/

    /*İlk Madde Iplik tr click popup*/
    $('#İlkMaddeIplik tbody tr').dblclick(function () {
        var elem = $(this);
        $('#modalPopup').modal('show')
    })
    /*İlk Madde Iplik tr click popup*/

    /*Ölculer tr click popup*/
    $('#olculer tbody tr').dblclick(function () {
        var elem = $(this);
        $('#modalPopup').modal('show')
    })
    /*Ölculer tr click popup*/

    /*talimatlar tr click popup*/
    $('#talimatlar tbody tr').dblclick(function () {
        var elem = $(this);
        $('#modalPopup').modal('show')
    })
    /*talimatlar tr click popup*/

    /*fiyatlar tr click popup*/
    $('#fiyatlar tbody tr').dblclick(function () {
        var elem = $(this);
        $('#modalPopup').modal('show')
    })
    /*fiyatlar tr click popup*/
})

//function checkbox() {
//    $('input[type="checkbox"]').iCheck({
//        checkboxClass: 'icheckbox_flat-blue',
//        radioClass: 'iradio_flat-blue'
//    });
//}
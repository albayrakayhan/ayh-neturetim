$(document).ready(function () {
    
    /*tablet elemenets*/
    $(function () {
        $('#stokKart').DataTable({
            "paging": true,
            "lengthChange": false,
            "searching": false,
            "ordering": true,
            "info": true,
            "autoWidth": true
        });
    });
    /*tablet elemenets*/

    /*checkbox*/
    checkbox();
    /*checkbox*/

    /*tr check*/
    $('#stokKart tbody tr').click(function () {
        var elem = $(this).find('input[type=checkbox]')
        if (!elem.prop("checked")) {
            elem.prop("checked", true);
            checkbox(true);
        } else {
            elem.prop("checked", false);
            checkbox(false);
        }
    })
    /*tr check*/
    /*allCheckBox check*/
    $('.allCheckBox').click(function myfunction() {
        var elem = $('#stokKart tbody tr').find('input[type=checkbox]');
        if (!elem.prop("checked")) {
            elem.prop("checked", true);
            checkbox(true);
        } else {
            elem.prop("checked", false);
            checkbox(false);
        }
    })
    /*allCheckBox check*/

    /*dbclikc page go*/
    $('#stokKart tbody tr').dblclick(function (e) {
        var elem = $(this).attr('data-url');
        window.location.href = elem;

    })
    /*dbclikc page go*/
})
function checkbox() {
    $('input[type="checkbox"]').iCheck({
        checkboxClass: 'icheckbox_flat-blue',
        radioClass: 'iradio_flat-blue'
    });
}


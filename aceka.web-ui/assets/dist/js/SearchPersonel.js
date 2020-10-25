//$(document).ready(function () {

//personeller('personeller');

//function personeller(controlId) {
//    var metodUrl = 'api/personel/personel-arama?carikart_id=null&cari_unvan= null&ozel_kod= null&carikart_tipi_id=21&statu=True';
//    $.ajax({
//        type: "GET",
//        url: apiUrl + metodUrl,
//        dataType: "json",
//        success: function (data) {
//            $.each(data, function (key, obj) {
//                $('#' + controlId).append('<tr data-url="detail"> <td class="widthTable"><input type="checkbox"></td> <td>' + obj.tanim + '</td> <td>' + obj.personel_no + '</td><td>' + obj.statu + '</td> <td>' + obj.personeltipi + '</td> <td>' + obj.dogumtarihi + '</td> </tr>');
//            });

//            $('#personelKart').DataTable({
//                "paging": true,
//                "lengthChange": false,
//                "searching": false,
//                "ordering": true,
//                "info": true,
//                "autoWidth": true,
//            });

//            /*checkbox*/
//            checkbox();
//            /*checkbox*/

//            /*tr check*/
//            $('#personelKart tbody tr').click(function () {
//                var elem = $(this).find('input[type=checkbox]')
//                if (!elem.prop("checked")) {
//                    elem.prop("checked", true);
//                    checkbox(true);
//                } else {
//                    elem.prop("checked", false);
//                    checkbox(false);
//                }
//            })
//            /*tr check*/

//            /*allCheckBox check*/
//            $('.allCheckBox').click(function myfunction() {
//                var elem = $('#personelKart tbody tr').find('input[type=checkbox]');
//                if (!elem.prop("checked")) {
//                    elem.prop("checked", true);
//                    checkbox(true);
//                } else {
//                    elem.prop("checked", false);
//                    checkbox(false);
//                }
//            })
//            /*allCheckBox check*/

//            /*dbclikc page go*/
//            $('#personelKart tbody tr').dblclick(function (e) {
//                var elem = $(this).attr('data-url');
//                window.location.href = elem;

//            })
//            /*dbclikc page go*/
//        }
//    });
//};
//})

function checkbox() {
    $('input[type="checkbox"]').iCheck({
        checkboxClass: 'icheckbox_flat-blue',
        radioClass: 'iradio_flat-blue'
    });
}
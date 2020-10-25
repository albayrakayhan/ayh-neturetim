$(document).ready(function () {
    //$('#dosyaSec').on('change', function () {
    //    for (var i = 0; i < this.files.length; i++) {
    //        var elem = this.files[i].name
    //        $('.fileButton').text(elem);
    //    }
    //});
    /*stokKodu*/
    //$(function () {
    //    var availableTags = [
    //      "Ada Plastik",
    //      "Ara Nv",
    //      "Avery",
    //      "Bogaz içi tesktil",
    //    ];
    //    $("#varsayilanTedarikci").autocomplete({
    //        source: availableTags
    //    });
    //});
    /*stokKodu*/

    /*checkbox*/
    //checkbox();
    /*checkbox*/

    /*Genel image tablet elemenets*/
    //$(function () {
    //    $('#imageDownload').DataTable({
    //        "paging": true,
    //        "lengthChange": false,
    //        "searching": false,
    //        "ordering": true,
    //        "info": true,
    //        "autoWidth": true,
    //        "pageLength": 5,
    //    });
    //});
    /*Genel image tablet elemenets*/

    /*tr delete*/
    //$('.delete').click(function () {
    //    var checked = $('input[type="checkbox"]:checked').parent().parent().parent().remove();
    //})
    /*tr delete*/

    /*ilkMaddeKumas tablet elemenets*/
    //$(function () {
    //    $('.tableDesing').DataTable({
    //        "paging": true,
    //        "lengthChange": false,
    //        "searching": false,
    //        "ordering": true,
    //        "info": true,
    //        "autoWidth": true,
    //        "pageLength": 5,
    //    });
    //});
    /*ilkMaddeKumas tablet elemenets*/

    /*Genel imageDownload tr click popup*/
    //$('#imageDownload tbody tr').dblclick(function () {
    //    var elem = $(this);
    //    var text = elem.find('td').first().text();
    //    $('.imageItem a img').attr('src', text);
    //})
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
    $('.stokKoduList li').click(function () {
        this.remove();
    })
    /*multiple checkbox*/
    $(function () {
        var availableTags = [
          "ActionScript",
          "AppleScript",
          "Asp",
          "BASIC",
          "C",
          "C++",
          "Clojure",
          "COBOL",
          "ColdFusion",
          "Erlang",
          "Fortran",
          "Groovy",
          "Haskell",
          "Java",
          "JavaScript",
          "Lisp",
          "Perl",
          "PHP",
          "Python",
          "Ruby",
          "Scala",
          "Scheme"
        ];
        function split(val) {
            return val.split(/,\s*/);
        }
        function extractLast(term) {
            return split(term).pop();
        }

        $("#stokkart_otomatik_varyant")
          .on("keydown", function (event) {
              if (event.keyCode === $.ui.keyCode.TAB &&
                  $(this).autocomplete("instance").menu.active) {
                  event.preventDefault();
              }
          })
          .autocomplete({
              minLength: 0,
              source: function (request, response) {
                  response($.ui.autocomplete.filter(
                    availableTags, extractLast(request.term)));
              },
              focus: function () {
                  return false;
              },
              select: function (event, ui) {
                  var terms = split(this.value);
                  terms.pop();
                  terms.push(ui.item.value);
                  terms.push("");
                  this.value = terms.join(", ");
                  return false;
              }
          });
    });
    /*multiple checkbox*/
})

//function checkbox() {
//    $('input[type="checkbox"]').iCheck({
//        checkboxClass: 'icheckbox_flat-blue',
//        radioClass: 'iradio_flat-blue',
//        cursor: true,
//    });
//    /*stokVaryantKart table*/
//    $('#tek_varyant').on('ifClicked', function (event) {
//        $('#VaryantKart_wrapper').toggleClass('tableactive');
//        $('#stokkart_otomatik_varyant').val('');
//    })
//    /*stokVaryantKart table*/
//}
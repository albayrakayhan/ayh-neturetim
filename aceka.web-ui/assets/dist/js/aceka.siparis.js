var siparis = [];
var talimatCheckedValues = [];
var sipnotCheckedValues = [];

siparis.init = function () {
    //checkbox();
};
siparis.data = {
    talimatlar: [],
    notlar: []
};
siparis.helper = {
    getSiparisId() {
        return $("#siparis_id").val();
    },

    getStokkartId() {
        return $("#stok_kodu").attr("data-stokkartid");
    },

    getStokKodu() {
        return $("#stok_kodu").val();
    }
};
siparis.talimatlar = {
    init: function () {
    },
    build: function (element, data) {
        let dataSoruce = siparis.talimatlar.getDataSoruce(element, data);
        let columns = siparis.talimatlar.buildColumns(element);
        //Sipariş New de adetler ve fiyatlar Kendo gridini getiriyor.
        siparis.talimatlar.createGrid(element, columns, dataSoruce);
    },
    buildColumns: function (element) {
        var columns = [];
        columns.push({ title: "Sira", field: "sira_id", width: 80 });
        columns.push({
            title: "İşlem", field: "talimatturu_id", width: 200, template: function (data) {
                if (typeof (data.talimat_adi) == "undefined") {
                    return "";
                }
                if (typeof (data.talimat_adi) == "object") {
                    return data.talimat_adi.tanim;
                }
                return data.talimat_adi;

            }, editor: siparis.talimatlar.talimatDropDownEditor
        });
        columns.push({
            title: "Fasoncu", field: "fasoncu_carikart_id", width: 200,
            template: function (data) {
                if (typeof (data.cari_unvan) == "undefined") {
                    return "";
                }

                return data.cari_unvan;

            }, editor: siparis.talimatlar.fasoncuDropDownEditor
        });
        columns.push({
            title: "aciklama", field: "aciklama",
            editor: function (container, options) {
                $('<textarea data-text-field="aciklama" data-value-field="Value" data-bind="value:' + options.field + '" style="width: ' + (container.width() - 10) + 'px;height:' + (container.height() + 200) + 'px;overflow-wrap: break-word;" />')
                    .appendTo(container);
            },
            attributes: {
                "class": "table-cell",
                style: "text-align: left;"
            }
        });
        columns.push({ command: ["destroy"], title: "&nbsp;", width: "150px" });
        return columns;
    },
    getDataSoruce: function (element, initData) {
        var dataSource = new kendo.data.DataSource({
            transport: {
                read: function (e) {
                    if (e != null) {
                        e.success(initData);
                    }
                },
                create: function (e) {
                    let siparis_id;
                    if (window.location.pathname.indexOf("/new") > 1) {
                        siparis_id = $("#siparisKartSend").attr("data-siparis_id");
                    } else {
                        siparis_id = siparis.helper.getSiparisId();
                    }
                    let postData = e.data;
                    postData.siparis_id = siparis_id;
                    $.ajax({
                        method: 'POST',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        url: apiUrl + "api/siparis/talimat",
                        data: JSON.stringify([e.data]),
                        success: function (result) {
                            e.success(result);
                        }
                    });
                },
                update: function (e) {
                    $.ajax({
                        method: 'PUT',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        url: apiUrl + "api/siparis/talimatlar",
                        data: JSON.stringify([e.data]),
                        success: function (result) {
                            e.success(result);
                        }
                    });
                },
                destroy: function (e) {
                    $.ajax({
                        method: 'DELETE',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        url: apiUrl + "api/siparis/talimatlar",
                        data: JSON.stringify(e.data),
                        success: function (result) {
                            e.success(result);
                        }
                    });
                }
            },
            schema: {
                model: {
                    id: "siparis_id",
                    fields: {
                        talimatturu_id: {},
                        fasoncu_carikart_id: {}
                    }
                }
            }
        });
        return dataSource;
    },
    intEditor: function (container, options) {
        $('<input name="' + options.field + '" data-text-field="' + options.field + '" ' +
            'data-value-field="' + options.field + '" ' +
            'data-bind="value:' + options.field + '" ' +
            'data-format="' + options.format + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                spinners: false,
                decimals: options.field.indexOf("adet") > -1 ? 0 : 2,
                round: false,
            });
    },
    talimatDropDownEditor: function (container, options) {

        $('<input required name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: true,
                dataTextField: "tanim",
                dataValueField: "talimatturu_id",
                dataSource: {
                    transport: {
                        dataType: "json",
                        read: apiUrl + "api/parametre/talimat-liste",
                    },
                    schema: {
                        model: {
                            id: "talimatturu_id"
                        }
                    }
                },
                select: function (e) {
                    let grid = container.parents(".k-grid").data("kendoGrid");
                    let trElement = container.parents("tr");
                    let trIndex = trElement.index();
                    var tdCell = container.next();

                    let row = grid.dataSource.at(trIndex);
                    console.log(e.dataItem);
                    console.log(row);
                    row.set("talimat_adi", e.dataItem.tanim);

                }

            });
    },
    fasoncuDropDownEditor: function (container, options) {
        $('<input required name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: true,
                dataTextField: "cari_unvan",
                dataValueField: "carikart_id",
                dataSource: {
                    transport: {
                        dataType: "json",
                        read: apiUrl + "api/modelkart/fasoncular",
                    },
                    schema: {
                        model: {
                            id: "carikart_id"

                        }
                    }
                },
                select: function (e) {
                    let grid = container.parents(".k-grid").data("kendoGrid");
                    let trElement = container.parents("tr");
                    let trIndex = trElement.index();
                    var tdCell = container.next();

                    let row = grid.dataSource.at(trIndex);
                    //console.log(e.dataItem.tanim);
                    //console.log(row);
                    row.set("cari_unvan", e.dataItem.cari_unvan);



                }
            });
    },
    //textareaEditor: function (container, options) {
    //    $('<textarea data-bind="value: ' + options.field + '" cols="50" rows="4"></textarea>')
    //        .appendTo(container);
    //},

    //textareaEditor: function (container, options) {
    //    $('<textarea data-text-field="Label" data-value-field="Value" data-bind="value:' + options.field + '" style="width: ' + (container.width() - 10) + 'px;height:' + (container.height() - 12) + 'px col="100"" />')
    //    //$('<textarea name="' + options.field + '" style="width: ' + container.width() + 'px;height:' + container.height() + 'px col="50" />')
    //    .appendTo(container);
    //},
    createGrid: function (element, columns, dataSoruce) {
        $("#" + element).kendoGrid({
            //dataBound: onDataBound,
            columns: columns,
            dataSource: dataSoruce,
            //scrollable: {
            //    endless: true Yeni çıkmış. Scroll u aşağı çektikçe kayıtları getiriyor. Uzun yerlerde kullanalım.
            //},
            scrollable: true,
            resizable: true,
            editable: true,
            navigatable: true,
            toolbar: ["create"]
        });
        //function onDataBound() {
        //    var grid = $("#" + element).data("kendoGrid");
        //    for (var i = 0; i < grid.columns.length; i++) {
        //        grid.autoFitColumn(i);
        //    }
        //}
    }
};
siparis.notlar = {
    init: function () {
    },
    build: function (element, data) {
        let dataSoruce = siparis.notlar.getDataSoruce(element, data);
        let columns = siparis.notlar.buildColumns(element);
        siparis.notlar.createGrid(element, columns, dataSoruce);
    },
    buildColumns: function (element) {
        var columns = [];
        columns.push({ title: "Sira", field: "sira_id", width: 80 });
        columns.push({ title: "aciklama", field: "aciklama", width: 400 });
        columns.push({ command: ["destroy"], title: "&nbsp;", width: "150px" });
        return columns;
    },
    getDataSoruce: function (element, initData) {
        var dataSource = new kendo.data.DataSource({
            transport: {
                read: function (e) {
                    if (initData!=null) {
                        e.success(initData);
                    }
                },
                create: function (e) {
                    let siparis_id;
                    if (window.location.pathname.indexOf("/new") > 1) {
                        siparis_id = $("#siparisKartSend").attr("data-siparis_id");
                    } else {
                        siparis_id = siparis.helper.getSiparisId();
                    }
                    let postData = e.data;
                    postData.siparis_id = siparis_id;
                    $.ajax({
                        method: 'POST',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        url: apiUrl + "api/siparis/siparis-notlar",
                        data: JSON.stringify([e.data]),
                        success: function (result) {
                            e.success(result);
                        }
                    });
                },
                update: function (e) {
                    $.ajax({
                        method: 'PUT',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        url: apiUrl + "api/siparis/siparis-notlar",
                        data: JSON.stringify([e.data]),
                        success: function (result) {
                            e.success(result);
                        }
                    });
                },
                destroy: function (e) {
                    $.ajax({
                        method: 'DELETE',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        url: apiUrl + "api/siparis/siparis-notlar",
                        data: JSON.stringify(e.data),
                        success: function (result) {
                            e.success(result);
                        }
                    });
                }
            },
            schema: {
                model: {
                    id: "siparis_id"
                }
            }
        });
        return dataSource;
    },
    createGrid: function (element, columns, dataSoruce) {
        $("#" + element).kendoGrid({
            columns: columns,
            dataSource: dataSoruce,
            scrollable: true,
            resizable: true,
            editable: true,
            navigatable: true,
            toolbar: ["create"]
        });
    }

    //init: function () {
    //    siparis.notlar.notlarGrid("notlarKendo", 0);
    //},
    //notlarGrid: function (element, id) {
    //    var columns = [];
    //    columns.push({ title: "Sıra", field: "sira_id", width: 40, type: "number" });
    //    columns.push({ title: "Açıklama", field: "aciklama", width: 100, type: "string" });
    //    columns.push({ command: ["destroy"], title: "&nbsp;", width: "150px" });
    //    $("#notlarKendo").kendoGrid({
    //        columns: columns,
    //        dataSource: {
    //            transport: {
    //                read: function (e) {
    //                    console.log("notlar Kendo E-", e);
    //                    $.ajax({
    //                        url: apiUrl + "api/siparis/siparis-notlar/" + id,
    //                        method: "GET",
    //                        headers: {
    //                            "content-type": "application/json",
    //                            "cache-control": "no-cache"
    //                        },
    //                        dataType: 'Json'
    //                    })
    //                      .success(function (data) {
    //                          e.success(data);
    //                      })
    //                },
    //                create: function (e) {
    //                    let siparis_id;
    //                    if (window.location.pathname.indexOf("/new") > 1) {
    //                        siparis_id = $("#siparisKartSend").attr("data-siparis_id");
    //                    } else {
    //                        siparis_id = siparis.helper.getSiparisId();
    //                    }
    //                    let postData = e.data;
    //                    postData.siparis_id = siparis_id;
    //                    $.ajax({
    //                        method: 'POST',
    //                        headers: {
    //                            "content-type": "application/json",
    //                            "cache-control": "no-cache"
    //                        },
    //                        url: apiUrl + "api/siparis/siparis-notlar",
    //                        data: JSON.stringify([e.data]),
    //                        success: function (result) {
    //                            e.success(result);
    //                        }
    //                    });
    //                },
    //                //create: function (e) {
    //                //    $.ajax({
    //                //        async: true,
    //                //        url: apiUrl + "api/siparis/siparis-notlar",
    //                //        method: "POST",
    //                //        headers: {
    //                //            "content-type": "application/json",
    //                //            "cache-control": "no-cache"
    //                //        },
    //                //        dataType: 'Json',
    //                //        data: JSON.stringify(e.data)
    //                //    })
    //                //      .success(function (data) {
    //                //          $("#notlarKendo")
    //                //            .data("kendoGrid")
    //                //            .dataSource.read();
    //                //      })
    //                //      .error(function (jqXHR, exception) {
    //                //          var errorJson = JSON.parse(jqXHR.responseText);
    //                //          genel.timer(
    //                //            300,
    //                //            'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");'
    //                //          );
    //                //      })
    //                //      .always(function () {
    //                //          kendo.ui.progress($("#notlarKendo"), false);
    //                //      });
    //                //},
    //                update: function (e) {
    //                    $.ajax({
    //                        url: apiUrl + "api/siparis/siparis-notlar",
    //                        method: "PUT",
    //                        headers: {
    //                            "content-type": "application/json",
    //                            "cache-control": "no-cache"
    //                        },
    //                        type: "json",
    //                        dataType: "json",
    //                        data: JSON.stringify([e.data])
    //                    })
    //                      .success(function (data) {
    //                          e.success();
    //                          $("#notlarKendo")
    //                            .data("kendoGrid")
    //                            .dataSource.read();
    //                      })
    //                      .error(function (jqXHR, exception) {
    //                          var errorJson = JSON.parse(jqXHR.responseText);
    //                          genel.timer(
    //                            300,
    //                            'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");'
    //                          );
    //                      });
    //                },
    //                destroy: function (e) {
    //                    $.ajax({
    //                        method: 'DELETE',
    //                        headers: {
    //                            "content-type": "application/json",
    //                            "cache-control": "no-cache"
    //                        },
    //                        url: apiUrl + "api/siparis/siparis-notlar",
    //                        data: JSON.stringify(e.data),
    //                        success: function (result) {
    //                            e.success(result);
    //                        }
    //                    });
    //                }
    //            },
    //            schema: {
    //                model: {
    //                    id: "siparis_id"
    //                }
    //            }
    //        },
    //        toolbar: ["create"],
    //        editable: true,
    //        //batch: true,
    //        //editable: {mode:'inline',createAt: "bottom" }
    //    });
    //}

};
siparis.adetler = {
    init: function () {
        kendo.ui.Grid.fn._dirtyCellTemplate = function (field, paramName) {
            var dirtyField;
            if (field && paramName) {
                dirtyField = field.charAt(0) === "[" ? kendo.expr(field, paramName + ".dirtyFields") : paramName + ".dirtyFields['" + field + "']";
                return "#= " + paramName + " && " + paramName + ".dirty && " + paramName + ".dirtyFields && " + dirtyField + " ? ' k-dirty-cell' : '' #";
            }
            return "";
        }
        kendo.ui.Grid.fn._dirtyIndicatorTemplate = function (field, paramName) {
            var dirtyField;
            if (field && paramName) {
                dirtyField = field.charAt(0) === "["
                    ? kendo.expr(field, paramName + ".dirtyFields") : paramName + ".dirtyFields['" + field + "']";
                return "#= " + paramName + " && " + paramName + ".dirty && " + paramName + ".dirtyFields && " + dirtyField +
                    " ? '<span class=\"k-dirty\"></span>' : '' #";
            }
            return "";
        };
        let self = this;
        let siparis_id = siparis.helper.getSiparisId();

        siparis.get.adetler(siparis_id, function (data) {
            self.build("adetGenelKendo", data);
        });

        siparis.get.fiyatlar(siparis_id, function (data) {
            self.build("fiyatGenelKendo", data);
        });


    },
    build: function (element, data) {
        if (typeof data == "undefined" || data.length < 1)
            data = [];
        columnsData = data.length > 0 ? data[0] : [];
        let dataSoruce = siparis.adetler.getDataSoruce(element, data);
        let columns = siparis.adetler.buildColumns(element, columnsData);
        //Sipariş New de adetler ve fiyatlar Kendo gridini getiriyor.
        siparis.adetler.createGrid(element, columns, dataSoruce);
    },
    buildColumns: function (element, columnsData) {
        var columns = [];
        columns.push({ title: "Beden", field: "beden", width: 200 });
        if (columnsData.siparisPivotArray instanceof Array) {
            columnsData.siparisPivotArray.forEach(function (item, i) {
                columns.push({
                    title: item.stok_kodu,
                    field: element === "adetGenelKendo"
                        ? "siparisPivotArray[" + i + "].adet"
                        : "siparisPivotArray[" + i + "].birimfiyat",
                    width: 100,
                    //format: element === "adetGenelKendo" ? "{0}" : "{0:c}",
                    //editor: siparis.adetler.intEditor
                });
            });
        }
        return columns;
    },
    getDataSoruce: function (element, initData) {
        var dataSource = new kendo.data.DataSource({
            transport: {
                read: function (e) {
                    e.success(initData);
                },
                /*
                create: function (e) {
                    $.ajax({
                        method: 'POST',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        url: apiUrl + "api/siparis/" + (element == "adetGenelKendo" ? "genel-adetler" : "genel-fiyatlar"),
                        data: JSON.stringify(e.data.models),
                        success: function (result) {
                            e.success(result);
                        }
                    });
                },
                */
                update: function (e) {
                    $.ajax({
                        method: 'PUT',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        url: apiUrl + "api/siparis/" + (element == "adetGenelKendo" ? "genel-adetler/list" : "genel-fiyatlar/list"),
                        data: JSON.stringify(e.data.models),
                        success: function (result) {
                            e.success(result);
                        }
                    });
                }
            },
            batch: true,
            schema: {
                model: {
                    id: "beden_id",
                    fields: {
                        beden: { editable: false, nullable: true },
                    }
                }
            }
        });
        return dataSource;
    },
    intEditor: function (container, options) {
        $('<input type="number" name="' + options.field + '" data-text-field="' + options.field + '" ' +
            'data-value-field="' + options.field + '" ' +
            'data-bind="value:' + options.field + '" ' +
            'data-format="' + options.format + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                spinners: false,
                decimals: options.field.indexOf("adet") > -1 ? 0 : 2,
                round: false,
            });
    },
    createGrid: function (element, columns, dataSoruce) {
        $("#" + element).kendoGrid({
            columns: columns,
            dataSource: dataSoruce,
            scrollable: true,
            resizable: true,
            editable: true,
            navigatable: true
        });
    }
}
siparis.search = {
    init: function () {
        $('.searchFrom').submit(function (e) {
            e.preventDefault();
        });
        $('.aramaSiparisBtn').on('click', function () {

            var metodUrl = "api/siparis/arama";
            var parameter = '?i==1';

            var siparis_no = $("#siparis_no").val();
            var musteri_carikart_id = $("#carikart_id").val();
            var sezon_id = $("#sezon_id").val();

            //var stok_kodu = $("#stokKodu").val();
            //var stokKodu = $("#stokKodu").attr('data-id') // -> Burası atanamıyor! sorun bura ile ilgili! autocomplate tarafında data-id işlemi olmuyor
            if ($("#stokKodu").attr('data-id') != undefined && $("#stokKodu").attr('data-id') != null) {
                var modelno = $("#stokKodu").attr('data-id');
            }
            else {
                var modelno = $("#stokKodu").val();
            }
            var modeladi = $("#stok_adi").val();
            var siparis_tarihi = $('#siparis_tarihi').val();

            //SiparisAra(string siparis_no = "", long musteri_carikart_id = 0, byte sezon_id = 0, string modelno = "", string modeladi = "", string baslangic_tarihi = "", string bitis_tarihi = "") 
            //var url = apiUrl + metodUrl + '?siparis_no=' + siparis_no + '&musteri_carikart_id=' + musteri_carikart_id + '&sezon_id=' + sezon_id + '&stok_kodu=' + stok_kodu + '&stok_adi=' + stok_adi + '&siparis_tarihi=' + siparis_tarihi;

            table = null;

            if (siparis_no.length > 0)
                parameter += '&siparis_no=' + encodeURIComponent(siparis_no);
            if (musteri_carikart_id.length > 0)
                parameter += '&musteri_carikart_id=' + encodeURIComponent(musteri_carikart_id);
            if (sezon_id.length > 0)
                parameter += '&sezon_id=' + encodeURIComponent(sezon_id);
            //if (stok_kodu.length > 0)
            //    parameter += '&stok_kodu=' + encodeURIComponent(stok_kodu);
            //if (stok_adi.length > 0)
            //    parameter += '&stok_adi=' + encodeURIComponent(stok_adi);

            //if (stok_kodu.length > 0) //Modelno
            //    parameter += '&stok_kodu=' + encodeURIComponent(stok_kodu);
            if (stokKodu.length > 0) //Modelno
                parameter += '&stokKodu=' + encodeURIComponent(stokKodu);

            if (modeladi.length > 0)
                parameter += '&modeladi=' + encodeURIComponent(modeladi);
            //if (stokKodu != undefined && stokKodu.length > 0)
            //    parameter += '&stokkart_id=' + encodeURIComponent(stokKodu);

            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl + parameter,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        table = $('#siparisKart').DataTable({
                            data: data,
                            paging: true,
                            destroy: true,
                            lengthChange: false,
                            searching: false,
                            ordering: true,
                            info: true,
                            autoWidth: true,
                            columns: [
                                {
                                    data: "siparis_id",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" name="siparistur" data-id="' + row.siparis_id + '" class="iCheck-helper ">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "siparis_no" },
                                { data: "musteri_carikart.cari_unvan" },
                                { data: "stok_kodu" },
                                { data: "stok_adi" },
                                {
                                    data: "siparis_tarihi",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return genel.dateFormat(row.siparis_tarihi);
                                        }
                                        return data;
                                    }
                                }

                            ],
                            initComplete: function (settings, json) {
                                //checkbox();
                                genel.iCheck('input[name="siparistur"]');
                            }
                        });

                        $('.paginate_button').on('click', function () {
                            //checkbox();
                            genel.iCheck('input[name="siparistur"]');
                        });
                    } else {
                        genel.modal("Dikkat!", "Kayıt bulunamadı", "uyari", "$('#myModal').modal('hide');");
                    }
                }
            });

            $('#siparisKart > tbody').on('dblclick', 'tr', function () {
                var data = table.row(this).data();
                //location = '/siparis/detail/' + data.siparis_id;
                $("#searchTab").attr("class", "tab-pane");
                $("#detailTab").attr("class", "active tab-pane");
                SiparisDetayYukle(data.siparis_id);
            });

        });
        $("#stok_adi").autocomplete({
            //source: ,
            source: function (request, response) {

                var data = stokkart.autocomplate.stokAdiTurListesi({ stokkart_tur_id: 0, stok_adi: request.term, stok_kodu: '' });
                response($.map(data, function (item) {
                    //return { label: item.code, value: item.value, id: item.id };
                    return { label: item.value, value: item.value, id: item.id };
                }));
            },
            minLength: 2,
            select: function (event, ui) {
                $("#stokKodu").attr('data-id', ui.item.id);
            },

        });
        $("#siparis_no").autocomplete({
            source: function (request, response) {
                var data = siparissearch.autocomplate.SiparisNoListe({ siparis_no: request.term });
                response($.map(data, function (item) {
                    return { label: item.value, value: item.value, id: item.id };
                    $("#siparis_no").attr('data-id', item.id);
                }));

            },
            minLength: 2,
            select: function (event, ui) {
                $("#siparis_no").attr('data-id', ui.item.id);
            }
        });
        $("#modelAdi").autocomplete({
            //source: ,
            source: function (request, response) {

                var data = stokkart.autocomplate.stokAdiTurListesi({ stokkart_tur_id: 0, stok_adi: request.term, stok_kodu: '' });
                response($.map(data, function (item) {
                    return { label: item.value, value: item.code, id: item.id };
                }));
            },
            minLength: 2
        });
    }
};

siparis.get = {
    init: function (id) {
        // alert('Model Kart No:' + id);
        $('.talimatBtn').attr("onclick", "siparis.get.talimatGetPopup(" + id + ")");
        $('.talimatDeleteBtn').attr("onclick", "siparis.delete.talimat(" + id + ")");
        $('.varyantBtn').attr("onclick", "siparis.get.varyantGetPopup(" + id + ")");
        $('.siparisnotBtn').attr("onclick", "siparis.get.siparisNotGetPopup(" + id + ")");
        $('.siparisnotDeleteBtn').attr("onclick", "siparis.delete.siparisnotlar(" + id + ")");
        $('.fileDeleteButton').attr("onclick", "modelkart.delete.ekler(" + id + ")");
        $('.varyantDeleteBtn').attr("onclick", "modelkart.delete.varyant(" + id + ")");
        $('.btnModelkartPopup').attr("onclick", "siparis.get.modelkartPopup(" + id + ")");
    },
    genelUst: function (id, secilecekParametreControlIds) {
        if (id > 0) {
            //api/siparis/{siparis_id}
            var metodUrl = "api/siparis/" + id;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: 'Json',
                async: false,
                success: function (data) {
                    //inputlar dolduruluyor.
                    $('#siparis_id').val(data.siparis_id);
                    console.log("siparişno:"+data.siparis_no);
                    $('#detay_siparis_no').val(data.siparis_no);
                    $('#siparis_tarihi').val(genel.dateFormat(data.siparis_tarihi));
                    $('#musteri_carikart_id').val(data.musteri_carikart_id);
                    $('#siparisturu_id').val(data.siparisturu_id);
                    $('#zorlukgrubu_id').val(data.zorlukgrubu_id);
                    $('#uretimyeri_id').val(data.uretimyeri_id);
                    $('#mense_uretimyeri_id').val(data.mense_uretimyeri_id);
                    $('#siparis_not').val(data.siparis_not);
                    $('#stok_kodu').val(data.stok_kodu).attr('data-stokkartid', data.stokkart_id);
                    $('#musterifazla').val(data.musterifazla);
                    $('#siparis_id').val(data.siparis_id);
                    $('#statu').val(data.statu === true ? 1 : 0);
                    $('#pb').val(data.pb);

                    //siparis_ozel

                    //$('#stok_kodu').val(data.siparis_ozel.stok_kodu);
                    $('#isteme_tarihi').val(genel.dateFormat(data.siparis_ozel.isteme_tarihi));
                    $('#tahmini_uretim_tarihi').val(genel.dateFormat(data.siparis_ozel.tahmini_uretim_tarihi));
                    $('#ref_siparis_no').val(data.siparis_ozel.ref_siparis_no);
                    $('#ref_siparis_no2').val(data.siparis_ozel.ref_siparis_no2);
                    genel.iCheck($("#ref_link_status").prop("checked", data.siparis_ozel.ref_link_status));
                    //$('#ref_link_status').val(data.siparis_ozel.ref_link_status)
                    $('#ref_link_status').val(data.siparis_ozel.ref_link_status)
                    $('#bayi_carikart_id').val(data.siparis_ozel.bayi_carikart_id);
                    $('#sezon_id').val(data.siparis_ozel.sezon_id);

                    //true ya da false döndüğü için sonuna toString() eklendi. Çünkü select'in value su bir string dir.
                    //$('#' + secilecekParametreControlIds.statu).val(data.statu.toString());

                    //Log için Onay buton status == true ise butonun text i "Onay İptal" Olarak değiştiriliyor!
                    //if (data.stokkart_onay.genel_onay) {
                    //    $('#onayButton').text('Onay İptal');
                    //    $('#onayButton').attr('data-status', 'false');
                    //} else {
                    //    $('#onayButton').attr('data-status', 'true');
                    //}

                    //Öntanımlı plug-in yükleniyor
                    $("#siparis_tarihi").tarihComponent();
                    $("#isteme_tarihi").tarihComponent();
                    $("#tahmini_uretim_tarihi").tarihComponent();

                    // İlk Madde (stokkart_tipi_id: Kumaş = 20, Aksesuar = 21, İplik = 22)
                    //siparis.get.ilkMadde(id, data.stokkart_id, 20, 'tableIlkMaddeKumas_Siparis');
                    //siparis.get.ilkMadde(id, data.stokkart_id, 21, 'tableIlkMaddeAksesuar_Siparis');
                    //siparis.get.ilkMadde(id, data.stokkart_id, 22, 'tableIlkMaddeIplik_Siparis');
                    //İlk Madde End
                }
            });
        }
    },
    ilkMaddeModel: function (id, stokkart_tipi_id) {
        var result = [];
        if (id > 0) {
            // api/modelkart/ilk-madde-modeller-pivot/{stokkart_id},{stokkart_tipi_id}
            var metodUrl = "api/siparis/ilk-madde-modeller-pivot/" + id + ',' + stokkart_tipi_id;
            var url = apiUrl + metodUrl;
            var stokkartTipi = "";
            switch (stokkart_tipi_id) {
                case 20:
                    stokkartTipi = "Kumaş"
                    break;
                case 21:
                    stokkartTipi = "Aksesuar"
                    break;
                case 22:
                    stokkartTipi = "İplik"
                    break;
            }
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        result = data;
                    }
                }
            });
        }
        return result;
    },
    ilkMadde: function (id, stokkart_id, stokkart_tipi_id, tableName) {

        let result = [];

        if (id > 0 && stokkart_id > 0) {
            //api/siparis/ilk-madde-modeller-pivot/{siparis_id},{stokkart_id},{stokkart_tipi_id}
            var metodUrl = "api/siparis/ilk-madde-modeller-pivot/" + id + ',' + stokkart_id + ',' + stokkart_tipi_id;
            var url = apiUrl + metodUrl;
            var stokkartTipi = "";
            switch (stokkart_tipi_id) {
                case 20:
                    stokkartTipi = "Kumaş"
                    break;
                case 21:
                    stokkartTipi = "Aksesuar"
                    break;
                case 22:
                    stokkartTipi = "İplik"
                    break;
            }
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        result = data;
                    }
                }
            });
        }
        return result;
    },
    talimatlar: function (id) {
        if (id > 0) {
            //api/siparis/talimatlar/{siparis_id}
            var metodUrl = "api/siparis/talimatlar/" + id;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    siparis.data.talimatlar[id] = data;
                }
                //success: function (data) {
                //    if (data != null) {
                //        talimatlarTable = $('#talimatlar').DataTable({
                //            data: data,
                //            paging: true,
                //            destroy: true,
                //            lengthChange: false,
                //            searching: false,
                //            ordering: true,
                //            info: true,
                //            autoWidth: true,
                //            columns: [
                //                {
                //                    data: "siparis_id",
                //                    render: function (data, type, row) {
                //                        if (type === 'display') {
                //                            return '<input type="checkbox" name="talimatsiparisCheckBox" data-sira_id="' + row.sira_id + '" value="' + row.siparis_id + '" class="iCheck-helper">';
                //                        }
                //                        return data;
                //                    }
                //                },
                //                { data: "sira_id" },
                //                { data: "talimat_adi" },
                //                { data: "cari_unvan" },
                //                { data: "aciklama" },
                //                { data: "irstalimat" },
                //                { data: "islem_sayisi" }
                //            ],
                //            initComplete: function (settings, json) {
                //                genel.iCheck('input[name="talimatsiparisCheckBox"]');
                //            }
                //        });
                //        $('.paginate_button').on('click', function () {
                //            //checkbox();
                //        });
                //        $('#talimatlar > tbody').on('dblclick', 'tr', function () {
                //            var data = talimatlarTable.row(this).data();
                //            var siparis_id = data.siparis_id;
                //            genel.talimatGet({ siparis_id: id, event: "siparis.put.talimatlar();", data: data });
                //        });
                //        $('input[name="talimatsiparisCheckBox"]').on('ifChecked', function (event) {
                //            var siparis_id = event.target.value;
                //            var sira_id = event.target.attributes["data-sira_id"].nodeValue;
                //            talimatCheckedValues.push({ siparis_id: siparis_id, sira_id: sira_id });
                //        });
                //        $('input[name="talimatsiparisCheckBox"]').on('ifUnchecked', function (event) {
                //            var siparis_id = event.target.value;
                //            var sira_id = event.target.attributes["data-sira_id"].nodeValue;
                //            var tmpArray = [];
                //            for (var item, i = 0; item = talimatCheckedValues[i++];) {
                //                if (item.sira_id != sira_id && item.siparis_id == siparis_id) {
                //                    tmpArray.push(item);
                //                }
                //            }
                //            talimatCheckedValues = tmpArray;
                //        });
                //    } else {
                //        //if (talimatlarTable != null) {
                //        //    talimatlarTable.destroy();
                //        //    $('#talimatlar > tbody').html('');
                //        //}
                //    }
                //}
            });
        }
    },
    siparisnotlar: function (id) {
        if (id > 0) {
            //api/siparis/siparis-notlar/{siparis_id}
            var metodUrl = "api/siparis/siparis-notlar/" + id;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    siparis.data.notlar[id] = data;
                }
                //success: function (data) {
                //    if (data != null) {
                //        siparisnotlarTable = $('#sipnotlar').DataTable({
                //            data: data,
                //            paging: true,
                //            destroy: true,
                //            lengthChange: false,
                //            searching: false,
                //            ordering: true,
                //            info: true,
                //            autoWidth: true,
                //            columns: [
                //                {
                //                    data: "siparis_id",
                //                    render: function (data, type, row) {
                //                        if (type === 'display') {
                //                            return '<input type="checkbox" name="sipNotCheckBox" data-sira_id="' + row.sira_id + '" value="' + row.siparis_id + '" class="iCheck-helper">';
                //                        }
                //                        return data;
                //                    }
                //                },
                //                { data: "sira_id" },
                //                { data: "aciklama" },
                //            ],
                //            initComplete: function (settings, json) {
                //                genel.iCheck('input[name="sipNotCheckBox"]');
                //            }, drawCallback: function () {
                //                $('.paginate_button')
                //                    .on('click', function () {
                //                        genel.iCheck('input[name="sipNotCheckBox"]');
                //                        var api = this.api();
                //                    });
                //            }
                //        });
                //        $('.paginate_button').on('click', function () {
                //            //checkbox();
                //        });
                //        $('#sipnotlar > tbody').on('dblclick', 'tr', function () {
                //            var data = siparisnotlarTable.row(this).data();
                //            var siparis_id = data.siparis_id;
                //            genel.siparisNotlarPopGet({ siparis_id: id, event: "siparis.put.siparisnotlar();", data: data });
                //        });
                //        $('input[name="sipNotCheckBox"]').on('ifChecked', function (event) {
                //            var siparis_id = event.target.value;
                //            var sira_id = event.target.attributes["data-sira_id"].nodeValue;
                //            sipnotCheckedValues.push({ siparis_id: siparis_id, sira_id: sira_id });
                //        });
                //        $('input[name="sipNotCheckBox"]').on('ifUnchecked', function (event) {
                //            var siparis_id = event.target.value;
                //            var sira_id = event.target.attributes["data-sira_id"].nodeValue;
                //            var tmpArray = [];
                //            for (var item, i = 0; item = sipnotCheckedValues[i++];) {
                //                if (item.sira_id != sira_id && item.siparis_id == siparis_id) {
                //                    tmpArray.push(item);
                //                }
                //            }
                //            sipnotCheckedValues = tmpArray;
                //        });
                //    } else {
                //        //if (varyantlarTable != null) {
                //        //    varyantlarTable.destroy();
                //        //    $('#VaryantSkuKart > tbody').html('');
                //        //}
                //    }
                //}
            });
        }
    },
    talimatGetPopup: function (id) {
        genel.talimatGet({ siparis_id: id, event: "siparis.post.talimatlar();", data: null });
    },
    siparisNotGetPopup: function (id) {
        genel.siparisNotlarPopGet({ siparis_id: id, event: "siparis.post.siparisnotlar();", data: null });
    },
    adetler: function (id, callback) {
        if (id > 0) {
            //api/siparis/genel-adetler/{siparis_id}
            var metodUrl = "api/siparis/genel-adetler/" + id;
            var url = apiUrl + metodUrl;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                success: function (data) {
                    callback(data);
                }
            });
        }
    },
    fiyatlar: function (id, callback) {
        if (id > 0) {
            //api/siparis/genel-fiyatlar/{siparis_id}
            var metodUrl = "api/siparis/genel-fiyatlar/" + id;
            var url = apiUrl + metodUrl;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                success: function (data) {
                    callback(data);
                }
            });
        }
    },
    modelkartPopup: function (id) {
        genel.modelkartSearchPopup({ siparis_id: id, event: "siparis.get.modelkartPopupSelectedItem('" + id + "');", data: null });

        $('#modelPopupForm').submit(function (e) {
            e.preventDefault();
            return false;
        });

        $('#modelPopupForm .btnModelPopup').on('click', function () {
            //var stokKoduPopup = $('#myModal').parent().find('#stokKodu').attr('data-id');

            var stokKoduPopup = $('#myModal').parent().find('#stokKodu').val();

            if (stokKoduPopup != undefined && stokKoduPopup != '') {

                var metodUrl = "api/modelkart/arama";
                var stokKartTipi = 0;
                // if (stokKartTipi == null || stokKartTipi == '')
                //Buraya ne gelecek bilemiyorum
                var url = apiUrl + metodUrl + '?stok_kodu=' + stokKoduPopup + '&stokkart_tipi_id=' + stokKartTipi;
                $.ajax({
                    type: "GET",
                    url: url,
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data != null) {
                            var bulletLI = '';
                            $.each(data, function (i, val) {
                                // bulletLI += '<li><div class="form-group col-sm-12"><input type="radio" id="modelKartPopupBullet_' + val.stokkart_id + '" data-id="' + val.stokkart_id + '" name="modelKartPopupBullet"><label for="modelKartPopupBullet_' + val.stokkart_id + '">' + val.stok_adi + ' / ' + val.stok_kodu + '</label> </div>';
                                bulletLI += '<li> ' + val.stok_adi + ' / ' + val.stok_kodu + '<i class="fa fa-close"></i></li>  </div>';

                            });
                            if (bulletLI != '') {
                                $('#myModal').parent().find('#modelkartPopupBullet').html(bulletLI);
                            }
                        }
                    }
                });

            }
            $('.model-no li i').click(function () {
                $(this).parent().remove();
            })
        });
    },
    modelkartPopupSelectedItem: function (id) {
        var modalForm = $('#myModal').parent().find('#modelPopupForm');
        var tekil_stokkartId = $('#myModal').parent().find('#stokKodu').attr('data-id');
        var secilen_stokkartId = $('input[name=modelKartPopupBullet]:checked', modalForm).attr('data-id');
        var siparisOzelListe = siparis.get.siparisOzelListe(id);
        //Eğer arama sonucu ile herhangi bir kayıt seçilmedi ve autocomplate ile kayıt bulundu ise ilgili stokkart_id yi seçilen stokkart_id olarak eşitliyoruz!
        if (secilen_stokkartId == undefined && tekil_stokkartId != '')
            secilen_stokkartId = tekil_stokkartId

        if (secilen_stokkartId != undefined) {

            //genel.findInObject metodu bir aray içerisinde kayıt bulmaya yarar!
            var stokkart = genel.findInObject(siparisOzelListe, 'stokkart_id', secilen_stokkartId);
            /*
            1. Seçilen Stokkart ID daha önce eklenmiş ise
            Uyarı popup ı çıkacak bilgiler yenilensin mi? 
            Evet ise eskiler silinip yerine yenileri eklenecek. Hayır ise hiç bir işlem yapılmayacak.
            */
            if (stokkart.length > 0) {
                var cnf = confirm('Seçtiğiniz model kart daha önce kullanılmış.\nVarolan modelkart değiştirilecek!\nOnaylıyor musunuz?');
                if (cnf) {
                    // Varolan ile değişim onaylandığında yapılacak işlem.
                    //1. Sipariş_Model tablosundan kayıtlar silinecek mi? Eğer silinecek ise Sipariş_Model tablosunda ki kayıtların hangi modelkart (stokkart) tan geldiğine ait bir field yok
                }
            }
            /*
            2. Seçilen Stokkart ID daha önce eklenmemiş ise (yeni ise)
            "stokkart_model" tablosundan kayıtlar getirilecek ve siparis_model tablosuna kaydettirilecek.
            */
            else {
                $('#stok_kodu').val($('#myModal').parent().find('#stokKodu').val()).attr('data-stokkartid', $('#myModal').parent().find('#stokKodu').attr('data-id'));
                $('#myModal').modal('hide');

            }
        }

        let stokkart_id = siparis.helper.getStokkartId();
        /*
        let grids = [
            {gridElement : "ilkMaddeKumasCon", tip_id : 20},
            {gridElement : "ilkMaddeAksesuarCon", tip_id : 21 },
            {gridElement : "ilkMaddeIplikCon", tip_id : 22}
        ]
         
        data = modelkart.get.ilkMaddeModel(modelkart.helper.getStokkartId(), ilkMadde.ilkMaddeTipleri[keyElement]);
     */

        modelkart.get.varyantlar(stokkart_id);
        modelkart.get.talimatlar(stokkart_id);


        let adetData = [];
        let talimatData = [];
        let tempAdetData = modelkart.data.varyantlar[stokkart_id];

        let TemptalimatData = modelkart.data.talimatlar[stokkart_id];
        if (tempAdetData != undefined) {
            tempAdetData.forEach(function (item, i) {
                let pivotData = {
                    adet: 0,
                    birimfiyat: 0,
                    stok_kodu: siparis.helper.getStokKodu(),
                    stokkart_id: stokkart_id
                };
                //console.log("temp data foreach", pivotData);
                adetData.push({
                    beden: item.beden_tanimi,
                    beden_id: item.beden_id,
                    siparisPivotArray: [pivotData]
                });
                //console.log("Adet Data",adetData)
            });
        }
     
        TemptalimatData.forEach(function (item, i) {

            talimatData.push({
                aciklama: item.aciklama,
                talimat_adi: item.talimat_adi,
                fasoncu_carikart_id: item.fasoncu_carikart_id,
                talimatturu_id: item.talimatturu_id,
                sira_id: item.sira_id,
                stokkart_id: item.stokkart_id,
                aciklama: item.aciklama,
                talimat_adi: item.talimat_adi,
                irstalimat: item.irstalimat,
                islem_sayisi: item.islem_sayisi,
                cari_unvan: item.cari_unvan
            });

            //console.log("Adet Data",adetData)
        });
        let genelTab = ["adetGenelKendo", "fiyatGenelKendo"];
        genelTab.forEach(element => {
            siparis.adetler.build(element, adetData);
        });
        siparis.talimatlar.build("talimatKendo", talimatData);

    },
    siparisOzelListe: function (id) {
        var result = [];
        if (id > 0) {
            //api/siparis/siparis-ozel/{siparis_id}
            var metodUrl = "api/siparis/siparis-ozel/" + id;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: 'Json',
                async: false,
                success: function (data) {
                    result = data
                }
            });
        }
        return result;
    },
    siparisKumas: function () {
        var postData = {};
        var metodUrl = "api/modelkart/ilk-madde-modeller-pivot/" + $('#stok_kodu').attr('data-stokkartid') + ',' + 20
        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: 'Json',
            async: false,
            success: function (data) {
                if (data != undefined) {
                    postData = {
                        "aciklama": data[0].aciklama,
                        "alt_stokkart_id": data[0].alt_stokkart_id,
                        "birim_adi": data[0].birim_adi,
                        "modelyeri": data[0].modelyeri,
                    }
                }
            }
        });
        return postData;
    }
},
siparis.post = {
        send: function () {
            $("#siparisKart").on('submit', function (e) {
                e.preventDefault();
                if ($(this).valid()) {
                    var metodUrl = "api/siparis";
                    var dataArray = $(this).serializeArray();
                    //console.log(dataArray);
                    //CheckBox lar serializeArray ile gelmediğinden farklı bir dizi içerisine kopyalandı
                    var serializedCheckbox = $('input:checkbox').map(function () {
                        return { name: this.name, value: this.value == 'on' ? true : false };// ? this.value : "false"
                    });
                    var dataObj = {};
                    $(dataArray).each(function (i, field) {
                        dataObj[field.name] = field.value;
                    });
                    //CheckBox lar da dataArray e atanıyor.
                    $(serializedCheckbox).each(function (i, field) {
                        if ($.trim(field.name) != '') {
                            dataObj[field.name] = field.value;
                        }
                    });
                    var postData = {
                        "statu": true,
                        "siparis_no": dataObj['siparis_no'],
                        "siparis_tarihi": genel.dateFormatByJavascript(dataObj['siparis_tarihi'], '.'),
                        "musteri_carikart_id": dataObj['musteri_carikart_id'],
                        "uretimyeri_id": dataObj['uretimyeri_id'],
                        "mense_uretimyeri_id": dataObj['mense_uretimyeri_id'],
                        "siparis_not": dataObj['siparis_not'],
                        "stok_kodu": dataObj['stok_kodu'],
                        "musterifazla": dataObj['musterifazla'],
                        "siparisturu_id": dataObj['siparisturu_id'],
                        "zorlukgrubu_id": dataObj['zorlukgrubu_id'],
                        "siparis_ozel": {
                            "stokkart_id": $('#stok_kodu').attr('data-stokkartid'),
                            "isteme_tarihi": genel.dateFormatByJavascript(dataObj['isteme_tarihi'], '.'),
                            "tahmini_uretim_tarihi": genel.dateFormatByJavascript(dataObj['tahmini_uretim_tarihi'], '.'),
                            "ref_siparis_no": dataObj['ref_siparis_no'],
                            "ref_siparis_no2": dataObj['ref_siparis_no2'],
                            "ref_link_status": dataObj['ref_link_status'],
                            "sezon_id": dataObj['sezon_id'],
                            "bayi_carikart_id": dataObj['bayi_carikart_id']
                        }
                    }
                    $.ajax({
                        url: apiUrl + metodUrl,
                        method: 'POST',
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        processData: false,
                        data: JSON.stringify(postData)
                    }).success(function (data) {

                        $("#siparisKartSend").attr("data-siparis_id", data.ret_val);

                        let adetData = $("#adetGenelKendo").data("kendoGrid").dataSource.data();
                        let fiyatData = $("#fiyatGenelKendo").data("kendoGrid").dataSource.data();
                        let talimatData = $("#talimatKendo").data("kendoGrid").dataSource.data();

                        let newDetayData = [];

                        getBirimFiyat = function (fiyatData, beden_id) {
                            let birimfiyat;
                            fiyatData.forEach(function (item, index) {
                                if (item.beden_id == beden_id) {
                                    birimfiyat = item.siparisPivotArray[0].birimfiyat
                                }
                            });
                            return birimfiyat;
                        }
                        adetData.map(function (item, i) {
                            let birimfiyat = getBirimFiyat(fiyatData, item.beden_id);
                            newDetayData.push({
                                siparis_id: data.ret_val,
                                beden_id: item.beden_id,
                                siparisPivotArray: [
                                    {
                                        stokkart_id: item.siparisPivotArray[0].stokkart_id,
                                        adet: item.siparisPivotArray[0].adet,
                                        birimfiyat: birimfiyat
                                    }
                                ]
                            });
                        });
                        $.ajax({
                            url: apiUrl + "api/siparis/genel/list",
                            method: 'POST',
                            headers: {
                                "content-type": "application/json",
                                "cache-control": "no-cache"
                            },
                            data: JSON.stringify(newDetayData)
                        }).success(function () {
                            console.log("genel kayıt yapıldı");
                        }).error(function (jqXHR, exception) {
                            var errorJson = JSON.parse(jqXHR.responseText);
                            errorMessage = errorJson.message;
                            genel.timer(300, 'genel.modal("Hata!", "\'"+errorMessage +"\'", "hata", "$(\'#myModal\').modal(\'hide\');");');
                        });
                        let grids = ["ilkMaddeKumasCon", "ilkMaddeAksesuarCon", "ilkMaddeIplikCon"];
                        grids.map(function (gridkey) {
                            let grid = $("#" + gridkey).data("kendoGrid");
                            if (grid != undefined) {
                                grid.dataSource.sync();
                            }
                        });

                        $("#talimatKendo").data("kendoGrid").dataSource.sync();
                        if ($("#notlarKendo").data("kendoGrid") != undefined) {
                            $("#notlarKendo").data("kendoGrid").dataSource.sync();
                        }
                        let newNotlarData = [];
                        let gridnotlar = $("#notlarKendo").data("kendoGrid");
                        let index = $("#notlarKendo").data("kendoGrid").dataSource.view().length;
                        for (var i = 0; i < index; i++) {
                            newNotlarData.push({
                                siparis_id: data.ret_val,
                                sira_id: gridnotlar.dataSource.data()[i].sira_id,
                                aciklama: gridnotlar.dataSource.data()[i].aciklama
                            })
                        };
                        $.ajax({
                            url: apiUrl + "api/siparis/siparis-notlar",
                            method: 'POST',
                            headers: {
                                "content-type": "application/json",
                                "cache-control": "no-cache"
                            },
                            data: JSON.stringify(newNotlarData)
                        }).success(function () {
                            console.log("genel kayıt yapıldı");
                        }).error(function (jqXHR, exception) {
                            var errorJson = JSON.parse(jqXHR.responseText);
                            errorMessage = errorJson.message;
                            console.log("error", errorMessage);
                            genel.timer(300, 'genel.modal("Hata!", "\'"+errorMessage +"\'", "hata", "$(\'#myModal\').modal(\'hide\');");');
                        });
                        genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide'); location = '/siparis/detail/" + data.ret_val + "'");
                    }).error(function (jqXHR, exception) {
                        var errorJson = JSON.parse(jqXHR.responseText);
                        genel.timer(500, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                    });

                }
            });

            /*
            $("#siparisKart").onkeypress(submit, function () {
                if (event.keycode == 13) {
                    documnet.getElementById('siparisKart:submit').click();
                    return false;
                }
            })
            */
        },
        talimatPost: function () {
            var metodUrl = "api/siparis/talimat";
            //var rex = /(<([^>]+)>)/ig;
            //var dataArray = JSON.stringify(tableToJSON($("#talimatlar"))).replace(rex, "").serializeArray();
            var dataArray = tableToJSON($("#talimatlar"));

            var postData = [];
            for (var i = 0; i < dataArray.length; i++) {
                var pstData = {
                    "siparis_id": $('#siparis_id').val(),
                    //"stokkart_id": dataObj["hfId"],
                    "sira_id": dataArray[i]["Sıra"],
                    "talimatturu_id": ($(dataArray[i]["Talimat"]).attr('data-talimatturu_id') != undefined ? $(dataArray[i]["Talimat"]).attr('data-talimatturu_id') : null), //$(dataArray[i]["Talimat"]).attr('data-talimatturu_id'), //dizi içindeki html den değeri aldık.
                    "fasoncu_carikart_id": ($(dataArray[i]["Fasoncu"]).attr('data-fasoncu_carikart_id')),
                    //"fasoncu_carikart_id": dataArray[i]["Fasoncu"],
                    "islem_sayisi": dataArray[i]["İşlem Sayısı"],
                    "aciklama": dataArray[i]["Açıklama"],
                    "irstalimat": dataArray[i]["İrsaliye Açıklama"],
                }
                postData.push(pstData);
            }
            $.ajax({
                async: true,
                crossDomain: true,
                url: apiUrl + metodUrl,
                method: 'POST',
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                processData: false,
                data: JSON.stringify(postData)
            });
        },
        talimatlar: function () {
            /*stokakrt_id ve sira_id unique alanlar. bir stokkart_id ye ait iki adet sira_id olamaz.*/
            $("#talimatForm").on('submit', function (e) {
                e.preventDefault();
                //siparis.post.talimatPost();
                if ($(this).valid()) {
                    $("#myModal").modal("hide");
                    var metodUrl = "api/siparis/talimatlar";
                    //var formData = $(this).serialize();
                    var dataArray = $(this).serializeArray();
                    var dataObj = {};
                    $(dataArray).each(function (i, field) {
                        dataObj[field.name] = field.value;
                    });
                    var postData = {
                        "siparis_id": $('#siparis_id').val(),
                        "sira_id": dataObj["sira_id"],
                        "talimatturu_id": dataObj["talimatturu_id1"],
                        "fasoncu_carikart_id": dataObj["fasoncu_carikart_id1"],
                        "islem_sayisi": dataObj["islem_sayisi"],
                        "aciklama": dataObj["aciklama"],
                        "irstalimat": dataObj["irstalimat"] //İrsaliye Açıklama
                    }
                    var siparis_id = $('#siparis_id').val();
                    $.ajax({
                        async: true,
                        crossDomain: true,
                        url: apiUrl + metodUrl,
                        method: 'POST',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        processData: false,
                        data: JSON.stringify(postData)
                    }).success(function (data) {
                        if (data != undefined && data.message == 'Successful') { //Buradaki "Successful" api/modelkart/talimatlar daki apiden dönen message ın değeri.
                            genel.timer(300, 'genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$(\'#myModal\').modal(\'hide\');");siparis.get.talimatlar(' + siparis_id + ');');
                            //datatable refresh ediliyor
                            //talimatlarTable.reload();

                        } else {
                            genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                        }
                    }).error(function (jqXHR, exception) {

                        var errorJson = JSON.parse(jqXHR.responseText);
                        genel.timer(300, 'genel.modal("Hata!", "' + errorJson.message + '", "hata", "$(\'#myModal\').modal(\'hide\');");');

                    });
                }
            });
            $("#talimatForm").trigger('submit');
        },
        siparisnotlar: function () {
            $("#siparisNotForm").on('submit', function (e) {
                e.preventDefault();
                if ($(this).valid()) {
                    $("#myModal").modal("hide");
                    var metodUrl = "api/siparis/siparis-notlar";
                    var dataArray = $(this).serializeArray();
                    var dataObj = {};
                    $(dataArray).each(function (i, field) {
                        dataObj[field.name] = field.value;
                    });
                    var postData = {
                        "siparis_id": $('#siparis_id').val(),
                        "sira_id": dataObj["sira_id"],
                        "aciklama": dataObj["aciklama"]
                    }
                    var siparis_id = $('#siparis_id').val();
                    $.ajax({
                        async: true,
                        crossDomain: true,
                        url: apiUrl + metodUrl,
                        method: 'POST',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        processData: false,
                        data: JSON.stringify(postData)
                    }).success(function (data) {
                        if (data != undefined && data.message == 'Successful') {
                            genel.timer(300, 'genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$(\'#myModal\').modal(\'hide\');");siparis.get.siparisnotlar(' + siparis_id + ');');
                        } else {
                            genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                        }
                    }).error(function (jqXHR, exception) {
                        var errorJson = JSON.parse(jqXHR.responseText);
                        genel.timer(300, 'genel.modal("Hata!", "' + errorJson.message + '", "hata", "$(\'#myModal\').modal(\'hide\');");');
                    });
                }
            });
            $("#siparisNotForm").trigger('submit');
        },

    };
siparis.put = {
    send: function () {
        $("#siparisKart").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                var metodUrl = "api/siparis";
                var dataArray = $(this).serializeArray();
                // console.log(dataArray);
                //CheckBox lar serializeArray ile gelmediğinden farklı bir dizi içerisine kopyalandı
                var serializedCheckbox = $('input:checkbox').map(function () {
                    return { name: this.name, value: this.value == 'on' ? true : false };// ? this.value : "false"
                });
                var dataObj = {};
                $(dataArray).each(function (i, field) {
                    dataObj[field.name] = field.value;
                });
                //CheckBox lar da dataArray e atanıyor.
                $(serializedCheckbox).each(function (i, field) {
                    if ($.trim(field.name) != '') {
                        dataObj[field.name] = field.value;
                    }
                });
                var siparis_id = dataObj['hfsiparis_id'];
                var postData = {
                    "siparis_id": dataObj['hfsiparis_id'],
                    "statu": dataObj['statu'],
                    "siparis_no": dataObj['siparis_no'],
                    "siparis_tarihi": dataObj['siparis_tarihi'] != null ? genel.dateFormatByJavascript(dataObj['siparis_tarihi'], '.') : null,
                    "musteri_carikart_id": dataObj['musteri_carikart_id'],
                    "uretimyeri_id": dataObj['uretimyeri_id'],
                    "mense_uretimyeri_id": dataObj['mense_uretimyeri_id'],
                    "siparis_not": dataObj['siparis_not'],
                    "stok_kodu": dataObj['stok_kodu'],
                    "musterifazla": dataObj['musterifazla'],
                    "siparisturu_id": dataObj['siparisturu_id'],
                    "zorlukgrubu_id": dataObj['zorlukgrubu_id'],
                    "siparis_ozel": {
                        "ref_siparis_no": dataObj['ref_siparis_no'],
                        "tahmini_uretim_tarihi": dataObj['tahmini_uretim_tarihi'] != null ? genel.dateFormatByJavascript(dataObj['tahmini_uretim_tarihi'], '.') : null,
                        "isteme_tarihi": dataObj['isteme_tarihi'] != null ? genel.dateFormatByJavascript(dataObj['isteme_tarihi'], '.') : null,
                        "ref_siparis_no2": dataObj['ref_siparis_no2'],
                        "sezon_id": dataObj['sezon_id'],
                        "stokkart_id": $('#stok_kodu').attr('data-stokkartid')
                    }
                }
                //var stokkart_id = $('#stok_kodu').attr('data-stokkartid');
                $.ajax({
                    async: true,
                    crossDomain: true,
                    url: apiUrl + metodUrl,
                    method: 'PUT',
                    headers: {
                        "content-type": "application/json",
                        "cache-control": "no-cache"
                    },
                    processData: false,
                    data: JSON.stringify(postData)
                }).success(function (data) {
                    if (data != undefined && data.message == 'successful') {
                        genel.modal("Tebrikler!", "Kayıt güncellendi", "basarili", "$('#myModal').modal('hide');");

                        // veriler db ile eşitleniyor 
                        let grids = ["adetGenelKendo", "fiyatGenelKendo", "ilkMaddeKumasCon", "ilkMaddeAksesuarCon", "ilkMaddeIplikCon", "talimatKendo", "notlarKendo"];

                        grids.map(function (gridkey) {
                            let grid = $("#" + gridkey).data("kendoGrid");
                            if (grid != undefined) {
                                grid.dataSource.sync();
                            }
                        });

                        //siparis.post.talimatPost();
                        //siparis.get.genelUst(siparis_id);
                        //siparis.put.ilkMadde();
                    } else {
                        genel.modal("Hata!", "Güncelleme işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                        //genel.timer(300, 'genel.modal("Hata!", "\'"+errorMessage +"\'", "hata", "$(\'#myModal\').modal(\'hide\');");');
                    }
                }).error(function (jqXHR, exception) {
                    var errorJson = JSON.parse(jqXHR.responseText);
                    errorMessage = errorJson.message;
                    genel.timer(300, 'genel.modal("Hata!", "\'"+errorMessage +"\'", "hata", "$(\'#myModal\').modal(\'hide\');");');
                });
            }
        });
    },
    talimatlar: function () {
        /*stokkart_id ve sira_id unique alanlar. bir stokkart_id ye ait iki adet sira_id olamaz.*/
        $("#talimatForm").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                $("#myModal").modal("hide");
                var metodUrl = "api/siparis/talimatlar";
                //var formData = $(this).serialize();
                var dataArray = $(this).serializeArray();
                // console.log(dataArray);
                var dataObj = {};
                $(dataArray).each(function (i, field) {
                    dataObj[field.name] = field.value;
                });
                var postData = {
                    "siparis_id": dataObj["siparis_id"],
                    "sira_id": dataObj["sira_id"],
                    "talimatturu_id": dataObj["talimatturu_id1"],
                    "fasoncu_carikart_id": dataObj["fasoncu_carikart_id1"],
                    "islem_sayisi": dataObj["islem_sayisi"],
                    "aciklama": dataObj["aciklama"],
                    "irstalimat": dataObj["irstalimat"],
                    "eski_sira_id": dataObj["eski_sira_id"]
                }
                var siparis_id = dataObj['siparis_id'];
                $.ajax({
                    async: true,
                    crossDomain: true,
                    url: apiUrl + metodUrl,
                    method: 'PUT',
                    headers: {
                        "content-type": "application/json",
                        "cache-control": "no-cache"
                    },
                    processData: false,
                    data: JSON.stringify(postData)
                }).success(function (data) {
                    if (data != undefined && data.message == 'Successful') { //Buradaki "Successful" api/modelkart/talimatlar daki apiden dönen message ın değeri.
                        genel.timer(300, 'genel.modal("Tebrikler!", "Kayıt Güncellendi", "basarili", "$(\'#myModal\').modal(\'hide\');");siparis.get.talimatlar(' + siparis_id + ');');
                    } else {
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    var errorJson = JSON.parse(jqXHR.responseText);
                    genel.timer(300, 'genel.modal("Hata!", "' + errorJson.message + '", "hata", "$(\'#myModal\').modal(\'hide\');");');

                });
                //datatable refresh ediliyor
                // talimatlarTable.reload();
            }
        });
        $("#talimatForm").trigger('submit');
    },
    siparisnotlar: function () {
        $("#siparisNotForm").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                $("#myModal").modal("hide");
                var metodUrl = "api/siparis/siparis-notlar";
                //var formData = $(this).serialize();
                var dataArray = $(this).serializeArray();
                // console.log(dataArray);
                var dataObj = {};
                $(dataArray).each(function (i, field) {
                    dataObj[field.name] = field.value;
                });
                //siparis_id	sira_id	degistiren_carikart_id	degistiren_tarih	aciklama
                var postData = {
                    "siparis_id": dataObj["siparis_id"],
                    "sira_id": dataObj["sira_id"],
                    "aciklama": dataObj["aciklama"],
                }
                var siparis_id = dataObj['siparis_id'];
                $.ajax({
                    async: true,
                    crossDomain: true,
                    url: apiUrl + metodUrl,
                    method: 'PUT',
                    headers: {
                        "content-type": "application/json",
                        "cache-control": "no-cache"
                    },
                    processData: false,
                    data: JSON.stringify(postData)
                }).success(function (data) {
                    if (data != undefined && data.message == 'successful') {
                        genel.timer(300, 'genel.modal("Tebrikler!", "Kayıt Güncellendi", "basarili", "$(\'#myModal\').modal(\'hide\');");siparis.get.siparisnotlar(' + siparis_id + ');');
                    } else {
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    var errorJson = JSON.parse(jqXHR.responseText);
                    genel.timer(300, 'genel.modal("Hata!", "' + errorJson.message + '", "hata", "$(\'#myModal\').modal(\'hide\');");');

                });
            }
        });
        $("#siparisNotForm").trigger('submit');
    },
    ilkMadde: function () {

        var metodUrl = "api/siparis/ilk-madde-modeller";
        var dataArray = $("div.siparisKumas *").serializeArray();
        var dataObj = {};
        $(dataArray).each(function (i, field) {
            dataObj[field.name] = field.value;
        });
        var postData = {
            "siparis_id": siparis_id,
            "sira_id": dataObj["sira_id"],
            "talimatturu_id": dataObj["talimatturu_id"],
            "modelyeri": dataObj["modelyeri"],
            "alt_stokkart_id": dataObj["alt_stokkart_id"],
            "renk_id": dataObj["renk_id"],
        }
        $.ajax({
            async: false,
            crossDomain: true,
            url: apiUrl + metodUrl,
            method: 'PUT',
            headers: {
                "content-type": "application/json",
                "cache-control": "no-cache"
            },
            processData: false,
            data: JSON.stringify(postData)
        }).success(function (data) {
            if (data != undefined && data.message == 'successful') {
                //         genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');");
            } else {
                //       genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
            }
        }).error(function (jqXHR, exception) {
            genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
        });
    },

};
siparis.delete = {
    talimat: function (id) {
        if (talimatCheckedValues.length > 0) {
            var metodUrl = "api/siparis/talimatlar";
            var errCount = 0;
            var errorMessage = '';

            for (var i = 0; i < talimatCheckedValues.length; i++) {
                $.ajax({
                    async: false,
                    crossDomain: true,
                    url: apiUrl + metodUrl,
                    method: 'DELETE',
                    headers: {
                        "content-type": "application/json",
                        "cache-control": "no-cache"
                    },
                    processData: false,
                    data: JSON.stringify(talimatCheckedValues[i])
                }).success(function (data) {
                    if (data != undefined && data.message == 'Successful') {


                    } else {
                        errCount++;
                        errorMessage = "Silme işlemi yapılırken bir hata oluştu!";
                        genel.modal("Hata!", "Silme işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    errCount++;
                    var errorJson = JSON.parse(jqXHR.responseText);
                    errorMessage = errorJson.message;
                });
                if (errCount > 0) {
                    break;
                }
            }
            if (errCount > 0) {
                genel.timer(300, 'genel.modal("Hata!", "' + errorMessage + '", "hata", "$(\'#myModal\').modal(\'hide\');");');
            } else {

            }
            /*Hata olsun ya da olmasın checkboxlar uncheck ediliyor ve dizi boşaltılıyor*/
            talimatCheckedValues = [];
            siparis.get.talimatlar(id);
        }

    },
    siparisnotlar: function (id) {
        if (sipnotCheckedValues.length > 0) {

            var metodUrl = "api/siparis/siparis-notlar";
            var errCount = 0;
            var errorMessage = '';

            for (var i = 0; i < sipnotCheckedValues.length; i++) {
                $.ajax({
                    async: false,
                    crossDomain: true,
                    url: apiUrl + metodUrl,
                    method: 'DELETE',
                    headers: {
                        "content-type": "application/json",
                        "cache-control": "no-cache"
                    },
                    processData: false,
                    data: JSON.stringify(sipnotCheckedValues[i])
                }).success(function (data) {
                    if (data != undefined && data.message == 'Successful') {


                    } else {
                        errCount++;
                        errorMessage = "Kayıt işlemi yapılırken bir hata oluştu!";
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    errCount++;
                    var errorJson = JSON.parse(jqXHR.responseText);
                    errorMessage = errorJson.message;
                });
                if (errCount > 0) {
                    break;
                }
            }
            if (errCount > 0) {
                genel.timer(300, 'genel.modal("Hata!", "' + errorMessage + '", "hata", "$(\'#myModal\').modal(\'hide\');");');
            } else {

            }
            /*Hata olsun ya da olmasın checkboxlar uncheck ediliyor ve dizi boşaltılıyor*/
            sipnotCheckedValues = [];
            siparis.get.siparisnotlar(id);
        }

    },
};
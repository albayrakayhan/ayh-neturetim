var genelayarlar = [];

//var metodUrl = "api/modelkart/" + id;
//$.ajax({
//	type: "GET",
//	url: apiUrl + metodUrl,
//	dataType: "json",
//	async: false,
//	success: function (data) {
//		if (data != null) {
//			$('#stokkart_id').val(data.stokkart_id); //veri tabanındaki stokkart_id elemanına değerini atıyoruz.
//			$('#stokkart_tipi_id').val(data.stokkart_tipi_id);
//			$('#stok_kodu').val(data.stok_kodu);
//			}
//		}
//	}
//});
genelayarlar.get = {
  init: function(id) {
    $(".takvimBtn").attr("onclick", "genelayarlar.get.calismaTakvimi()");
  },
  GtiptanimlarGet: {
    init: function() {
      genelayarlar.get.GtiptanimlarGet.gtipGrid("grid");
    },
    gtipGrid: function(element) {
      $(document).ready(function(id) {
        $("#grid").kendoGrid({
          dataSource: {
            transport: {
              read: function(e) {
                console.log(e);
              },

              update: function(e) {
                $.ajax({
                  url: apiUrl + "api/genelayarlar/gtipput",
                  method: "PUT",
                  headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                  },
                  data: JSON.stringify(e.data),
                  dataType: "json"
                })
                  .success(function(data) {
                    e.success(e.data);
                  })
                  .always(function() {
                    kendo.ui.progress($(grid), false);
                  });
              }
            },
            schema: {
              model: {
                id: "belge_id"
              }
            }
          },
          pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 10,
            pageSize: 50,
            pageSizes: [25, 50, 100, 200, 250]
          },
          selectable: "row",
          resizable: true,
          editable: "inline",
          filterable: {
            mode: "row"
          },
          columns: [
            {
              field: "gtip_genel",
              title: "GTip No Genel(8 Hane)",
              attributes: { class: "table-cell", style: "text-align: center;" },
              filterable: {
                cell: { operator: "contains", suggestionOperator: "contains" }
              }
            },
            {
              field: "gtip_bayan",
              title: "GTip No Bayan(8 Hane)",
              attributes: { class: "table-cell", style: "text-align: center;" },
              filterable: {
                cell: { operator: "contains", suggestionOperator: "contains" }
              }
            },
            {
              field: "aciklama",
              title: "İthal Edilecek Hammadde Adı",
              attributes: { class: "table-cell" },
              width: 200,
              filterable: {
                cell: { operator: "contains", suggestionOperator: "contains" }
              }
            },
            {
              field: "birim",
              title: "Birim",
              template: function(data) {
                return data.birim_adi;
              },
              editor: function(container, options) {
                $(
                  '<input id="birim" data-text-field="birim_adi" data-value-field="birim_id" data-bind="value:' +
                    options.field +
                    '"/>'
                )
                  .appendTo(container)
                  .kendoDropDownList({
                    autoBind: true,
                    optionLabel: "Birim Seçiniz ",
                    dataTextField: "birim_adi",
                    dataValueField: "birim",
                    dataSource: {
                      transport: {
                        dataType: "jsonp",
                        read: apiUrl + "api/admin/birimliste"
                      }
                    },
                    dataBound: function(e) {
                      console.log(options, e);
                      this.value(options.model.birim_id);
                      this.trigger("select");
                      this.text(options.model.birim_adi);
                    }
                  });
              }
            },
            {
              field: "adet",
              title: "Adet",
              attributes: { class: "table-cell", style: "text-align: right;" },
              filterable: {
                cell: { operator: "contains", suggestionOperator: "contains" }
              }
            },
            {
              field: "kg",
              title: "Kg",
              attributes: { class: "table-cell", style: "text-align: right;" },
              filterable: {
                cell: { operator: "contains", suggestionOperator: "contains" }
              }
            },
            {
              field: "birim_fob",
              title: "Birim FOB $",
              attributes: { class: "table-cell", style: "text-align: right;" },
              filterable: {
                cell: { operator: "contains", suggestionOperator: "contains" }
              }
            },
            {
              field: "toplam_fob",
              title: "Toplam FOB $",
              attributes: { class: "table-cell", style: "text-align: right;" },
              filterable: {
                cell: { operator: "contains", suggestionOperator: "contains" }
              }
            },
            {
              command: ["edit"],
              title: "İşlemler",
              width: 180
            }
          ]
        });

        $(".clearSelection").click(function() {
          $("#grid")
            .data("kendoGrid")
            .clearSelection();
        });

        var selectRow = function(e) {
            if (e.type != "keypress" || kendo.keys.ENTER == e.keyCode) {
              var grid = $("#grid").data("kendoGrid"),
                rowIndex = $("#selectRow").val(),
                row = grid.tbody.find(">tr:not(.k-grouping-row)").eq(rowIndex);

              grid.select(row);
            }
          },
          toggleGroup = function(e) {
            if (e.type != "keypress" || kendo.keys.ENTER == e.keyCode) {
              var grid = $("#grid").data("kendoGrid"),
                rowIndex = $("#groupRow").val(),
                row = grid.tbody.find(">tr.k-grouping-row").eq(rowIndex);

              if (row.has(".k-i-collapse").length) {
                grid.collapseGroup(row);
              } else {
                grid.expandGroup(row);
              }
            }
          };

        $(".selectRow").click(selectRow);
        $("#selectRow").keypress(selectRow);

        $(".toggleGroup").click(toggleGroup);
        $("#groupRow").keypress(toggleGroup);
      });
    }
  },
  gtiparama: function() {
    var methodUrl = "api/genelayarlar/gtiparama";
    var stokKartTipi = $("#stokKartTipi").val();
    var modelTipi = $("#stokalan_id_5").val();
    var kumasTipi = $("#stokalan_id_2").val();
    var gtipIcerigi = $("#gtipIcerik").val();
    var gtipBelgeTuru = $("#gtipBelge").val();
    var kullanilanKayitlar = $("#sadeceKayitYukle").checked;
    var aciklamaParametreden = $("#aciklamaParametreden").checked;
    if (kumasTipi == null || kumasTipi == "") {
      kumasTipi = 0;
    }
    if (modelTipi == null || modelTipi == "") {
      modelTipi = -1;
    }
    $.ajax({
      url: apiUrl + methodUrl,
      type: "GET",
      headers: {
        "content-type": "application/json",
        "cache-control": "no-cache"
      },
      data: {
        stokkart_tipi_id: stokKartTipi,
        belge_id: gtipBelgeTuru,
        modeltipi_id: modelTipi,
        kumastipi_id: kumasTipi
      },
      success: function(data, textStatus, xhr) {
        $("#grid")
          .data("kendoGrid")
          .dataSource.data([]);
        $("#grid")
          .data("kendoGrid")
          .dataSource.data(data);
        $("#grid")
          .data("kendoGrid")
          .refresh();
      }
    });
  },
  kurlar: function() {
    var metodUrl = "api/kur/";
    var pb = $("#pb").val();
    var sene = $("#sene").val();
    var ay = $("#ay").val();
    if (sene != "" && ay != "" && pb != "") {
      //Tarih javascript formata dönüştürülüyor Ayhan ALBAYRAK
      //tarih = genel.dateFormatByJavascript(tarih, '.').split(':')[0].split('T')[0];

      $.ajax({
        type: "GET",
        //api/kur/{sene}/{ay}/{pb}
        url: apiUrl + metodUrl + sene + "/" + ay + "/" + pb,
        dataType: "json",
        async: false,
        success: function(data) {
          if (data != null) {
            table = $("#dovizTable").DataTable({
              data: data,
              paging: true,
              destroy: true,
              lengthChange: false,
              searching: false,
              ordering: true,
              info: true,
              autoWidth: true,
              iDisplayLength: 40,
              columns: [
                {
                  data: "tarih",
                  render: function(data, type, row) {
                    if (type === "display") {
                      return genel.dateFormat(row.tarih);
                    }
                    return data;
                  }
                },
                //{ data: "pb" },
                { data: "mb_alis" },
                { data: "mb_satis" },
                { data: "ser_alis" },
                { data: "ser_satis" }
              ],
              initComplete: function(settings, json) {
                //checkbox();
              }
            });

            $(".paginate_button").on("click", function() {
              //checkbox();
            });
          } else {
            genel.modal(
              "Dikkat!",
              "Kayıt bulunamadı",
              "uyari",
              "$('#myModal').modal('hide');"
            );
          }
        }
      });
    }
  },
  calismaTakvimi: function() {
    var metodUrl = "api/GenelAyarlar/";
    var sene = $("#sene").val();
    if (sene != "") {
      $.ajax({
        type: "GET",
        //api/GenelAyarlar/{sene}
        url: apiUrl + metodUrl + sene,
        dataType: "json",
        async: false,
        success: function(data) {
          if (data != null) {
            table = $("#takvimTable").DataTable({
              data: data,
              paging: true,
              destroy: true,
              lengthChange: false,
              searching: false,
              ordering: true,
              info: true,
              autoWidth: true,
              iDisplayLength: 40,
              columns: [
                { data: "sene" },
                {
                  data: "tarih",
                  render: function(data, type, row) {
                    if (type === "display") {
                      return genel.dateFormat(row.tarih);
                    }
                    return data;
                  }
                },
                { data: "gun_adi" },
                { data: "hafta" }
              ],
              initComplete: function(settings, json) {
                //checkbox();
              }
            });

            $(".paginate_button").on("click", function() {
              //checkbox();
            });
          } else {
            genel.modal(
              "Dikkat!",
              "Kayıt bulunamadı",
              "uyari",
              "$('#myModal').modal('hide');"
            );
          }
        }
      });
    }
  },
  // Talimatlar Kendo UI Grid için GET - POST - PUT - DESTROY Metodları
  talimatlar: {
    init: function() {
      genelayarlar.get.talimatlar.talimatGrid("talimatTuru");
    },
    talimatGrid: function(element) {
      var columns = [];
      columns.push({ title: "Sıra", field: "sira", width: 40, type: "string" });
      columns.push({ title: "Talimat Kodu", field: "kod", width: 100 });
      columns.push({ title: "Talimat Adı", field: "tanim", width: 90 });
      columns.push({
        title: "Pasif",
        field: "statu",
        width: 50,
        type: "boolean",
        template:
          '<input class="brm iCheck-helper" type="checkbox" #= statu ? "checked=checked" : "" # disabled="disabled" ></input>'
      });
      columns.push({
        title: "Varsayılan",
        field: "varsayilan",
        width: 80,
        type: "boolean",
        template:
          '<input type="checkbox" #= varsayilan ? "checked=checked" : "" # disabled="disabled" ></input>'
      });
      // Renkler
      columns.push({
        title: "Renk",
        field: "renk_rgb",
        width: 75,
        buttons: true,
        template: function(data) {
          return (
            "<div id='color' style='background-color: " +
            data.renk_rgb +
            ";'>&nbsp;</div>"
          );
        },
        editor: function(container, options) {
          try {
            $(
              '<input name="' +
                options.field +
                '"  data-text-field="' +
                options.field +
                '" ' +
                'data-value-field="' +
                options.field +
                '" ' +
                '"/>'
            )
              .appendTo(container)
              .kendoColorPicker({
                buttons: true,
                value: options.model.renk_rgb
              });
          } catch (error) {}
        }
      });
      columns.push({
        title: "Kesim",
        field: "kesim",
        width: 75,
        type: "boolean",
        template:
          '<input type="checkbox" #= kesim ? "checked=checked" : "" # disabled="disabled" ></input>'
      });
      columns.push({
        title: "Dikim",
        field: "dikim",
        width: 75,
        type: "boolean",
        template:
          '<input type="checkbox" #= dikim ? "checked=checked" : "" # disabled="disabled" ></input>'
      });
      columns.push({
        title: "Parça",
        field: "parca",
        width: 75,
        type: "boolean",
        template:
          '<input type="checkbox" #= parca ? "checked=checked" : "" # disabled="disabled" ></input>'
      });
      columns.push({
        title: "Model",
        field: "model",
        width: 75,
        type: "boolean",
        template:
          '<input type="checkbox" #= model ? "checked=checked" : "" # disabled="disabled" ></input>'
      });
      columns.push({
        title: "Model Giriş",
        field: "parcamodel_giris",
        width: 95,
        type: "boolean",
        template:
          '<input type="checkbox" #= parcamodel_giris ? "checked=checked" : "" # disabled="disabled" ></input>'
      });
      columns.push({
        title: "Model Çıkış",
        field: "parcamodel_cikis",
        width: 95,
        type: "boolean",
        template:
          '<input type="checkbox" #= parcamodel_cikis ? "checked=checked" : "" # disabled="disabled" ></input>'
      });
      columns.push({
        title: "Model Zorunlu",
        field: "model_zorunlu",
        width: 125,
        type: "boolean",
        template:
          '<input type="checkbox" #= model_zorunlu ? "checked=checked" : "" # disabled="disabled" ></input>'
      });
      // Fasoncular
      columns.push({
        title: "Varsayılan Fasoncu",
        field: "varsayilan_fasoncu",
        width: 150,
        template: function(data) {
          return data.cari_unvan;
        },
        editor: function(container, options) {
          $(
            '<input id="fasoncu" data-text-field="cari_unvan" data-value-field="carikart_id" data-bind="value:' +
              options.field +
              '"/>'
          )
            .appendTo(container)
            .kendoDropDownList({
              autoBind: true,
              optionLabel: "Fasoncu Seçiniz",
              dataTextField: "cari_unvan",
              dataValueField: "carikart_id",
              dataSource: {
                transport: {
                  read: apiUrl + "api/talimat/fasoncular"
                }
              },
              dataBound: function(e) {
                //console.log(options, e);
                this.value(options.model.varsayilan_fasoncu);
                this.trigger("select");
                this.text(options.model.cari_unvan);
              }
            });
        }
      });
      columns.push({
        title: "Kdv Tevkifat",
        field: "kdv_tevkifat",
        width: 100,
        template: function(data) {
          return data.kdv_tevkifat;
        },
        editor: function(container, options) {
          $(
            '<input id="kdv" data-text-field="oran" data-value-field="kdv_tevkifat" data-bind="value:' +
              options.field +
              '"/>'
          )
            .appendTo(container)
            .kendoDropDownList({
              autoBind: true,
              optionLabel: "Kdv Seçiniz ",
              dataTextField: "oran",
              dataValueField: "kod",
              dataSource: {
                transport: {
                  read: apiUrl + "api/admin/kdv"
                }
              },
              dataBound: function(e) {
                //console.log(options, e);
                this.value(options.model.kdv_tevkifat);
                this.trigger("select");
                this.text(options.model.kdv_tevkifat);
              }
            });
        }
      });
      columns.push({
        command: [{ name: "edit" }, { name: "destroy" }],
        title: "İşlemler",
        width: "175px"
      });
      $("#talimatTuru").kendoGrid({
        columns: columns,
        dataSource: {
          transport: {
            read: function(e) {
              $.ajax({
                url: apiUrl + "api/talimatlar",
                method: "GET",
                headers: {
                  "content-type": "application/json",
                  "cache-control": "no-cache"
                },
                type: "json",
                dataType: "json"
              })
                .success(function(data) {
                  console.log(data, "success");
                  e.success(data);
                })
                .error(function(jqXHR, exception) {
                  //console.log(exception);
                  var errorJson = JSON.parse(jqXHR.responseText);
                  genel.timer(
                    300,
                    'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");'
                  );
                });
            },
            create: function(e) {
              $.ajax({
                async: true,
                url: apiUrl + "api/talimatlar",
                method: "POST",
                headers: {
                  "content-type": "application/json",
                  "cache-control": "no-cache"
                },
                type: "json",
                dataType: "json",
                data: JSON.stringify(e.data)
              })
                .success(function(data) {
                  $("#talimatTuru")
                    .data("kendoGrid")
                    .dataSource.read();
                })
                .error(function(jqXHR, exception) {
                  var errorJson = JSON.parse(jqXHR.responseText);
                  genel.timer(
                    300,
                    'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");'
                  );
                })
                .always(function() {
                  kendo.ui.progress($("#talimatTuru"), false);
                });
            },
            update: function(e) {
              $.ajax({
                url: apiUrl + "api/talimatlar",
                method: "PUT",
                headers: {
                  "content-type": "application/json",
                  "cache-control": "no-cache"
                },
                type: "json",
                dataType: "json",
                data: JSON.stringify(e.data)
              })
                .success(function(data) {
                  e.success();
                  $("#talimatTuru")
                    .data("kendoGrid")
                    .dataSource.read();
                })
                .error(function(jqXHR, exception) {
                  var errorJson = JSON.parse(jqXHR.responseText);
                  genel.timer(
                    300,
                    'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");'
                  );
                });
            },
            destroy: function(e) {
              $.ajax({
                url: apiUrl + "api/talimatlar",
                method: "DELETE",
                headers: {
                  "content-type": "application/json",
                  "cache-control": "no-cache"
                },
                type: "json",
                dataType: "json",
                data: JSON.stringify(e.data)
              })
                .success(function(data) {
                  e.success();
                  $("#talimatTuru")
                    .data("kendoGrid")
                    .dataSource.read();
                })
                .error(function(jqXHR, exception) {
                  //console.log(exception);
                  var errorJson = JSON.parse(jqXHR.responseText);
                  genel.timer(
                    300,
                    'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");'
                  );
                });
            }
          },
          template: {},
          schema: {
            model: {
              id: "talimatturu_id",
              fields: {
                statu: { type: "boolean" },
                varsayilan: { type: "boolean" },
                kesim: { type: "boolean" },
                dikim: { type: "boolean" },
                parca: { type: "boolean" },
                model: { type: "boolean" },
                parcamodel_giris: { type: "boolean" },
                parcamodel_cikis: { type: "boolean" },
                model_zorunlu: { type: "boolean" },
                varsayilan_fasoncu: { type: "number" }
              }
            }
          }
        },
        toolbar: ["create"],
        editable: { mode: "inline", createAt: "bottom" }
      });
    }
  },
  // Sistem Ayarları Genel Sekmesi Kendo UI Grid için Get Metodları
  sistemGenelGet: {
    init: function() {
      genelayarlar.get.sistemGenelGet.sistemGenelGrid("sistemGenel");
    },
    sistemGenelGrid: function(element) {
      var columns = [];
      columns.push({ title: "Ayar Adı", field: "ayaraciklama" });
      columns.push({ title: "Ayar Değer", field: "ayar" });

      function onChange(arg) {
        var grid = $("#sistemGenel").data("kendoGrid");
        var row = this.select().closest("tr");
        var rowIdx = $("tr", grid.tbody).index(row);
        var colIdx = this.select().index();
        alert(rowIdx + "-" + colIdx);
      }

      $("#sistemGenel").kendoGrid({
        columns: columns,
        editable: true,
        navigatable: true,
        edit: function(e) {
          e.container.find(".k-input").blur(function() {
            console.log(e);

            $.ajax({
              url: apiUrl + "api/genelayarlar/sistemayarlariput",
              method: "PUT",
              headers: {
                "content-type": "application/json",
                "cache-control": "no-cache"
              },
              data: JSON.stringify(e.model),
              dataType: "json"
            }).success(function(data) {
              e.success(e.data);
            });
          });
        },

        pageable: {
          refresh: true,
          pageSizes: true,
          buttonCount: 10,
          pageSize: 50,
          pageSizes: [25, 50, 100, 200, 250]
        },
        dataSource: {
          transport: {
            read: function(e) {
              $.ajax({
                url: apiUrl + "api/genelayarlar/sistemayarlari",
                method: "GET",
                headers: {
                  "content-type": "application/json",
                  "cache-control": "no-cache"
                },
                type: "json",
                dataType: "json"
              })
                .success(function(data) {
                  e.success(data);
                })
                .error(function(jqXHR, exception) {
                  //console.log(exception);
                  var errorJson = JSON.parse(jqXHR.responseText);
                  genel.timer(
                    300,
                    'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");'
                  );
                });
            }
          },
          update: function(e) {
            $.ajax({
              url: apiUrl + "api/genelayarlar/sistemayarlariput",
              method: "PUT",
              headers: {
                "content-type": "application/json",
                "cache-control": "no-cache"
              },
              data: JSON.stringify(e.data),
              dataType: "json"
            })
              .success(function(data) {
                e.success(e.data);
              })
              .always(function() {
                kendo.ui.progress($(grid), false);
              });
          },
          schema: {
            model: {
              id: "ayar",
              fields: {
                ayaraciklama: { editable: false }
              }
            }
          }
        }
      });
    }
  },
  sistemSiparisPlanlamaGet: {
    init: function() {
      genelayarlar.get.sistemSiparisPlanlamaGet.sistemGenelGrid(
        "sistemSiparisPlanlama"
      );
    },
    sistemGenelGrid: function(element) {
      var columns = [];
      columns.push({
        title: "Sipariş Adedi",
        columns: [
          { title: "Alt Değer", field: "" },
          { title: "Üst Değer", field: "" }
        ]
      });
      columns.push({ title: "Kesim Firesi", field: "" });
      columns.push({ title: "Kesim Fazlası", field: "" });
      columns.push({
        command: [{ name: "edit" }, { name: "destroy" }],
        title: "İşlemler",
        width: "175px"
      });
      $("#sistemSiparisPlanlama").kendoGrid({
        columns: columns,
        // dataSource: {
        //     transport: {
        //         read: function (e) {
        //             $.ajax({
        //                 url: apiUrl + "api/",
        //                 method: 'get',
        //                 headers: {
        //                     "content-type": "application/json",
        //                     "cache-control": "no-cache"
        //                 },
        //                 type: "json",
        //                 dataType: "json",
        //             }).success(function (data) {
        //                 console.log(data, "success")
        //                 e.success(data);
        //             }).error(function (jqXHR, exception) {
        //                 //console.log(exception);
        //                 var errorJson = JSON.parse(jqXHR.responseText);
        //                 genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
        //             });
        //         },
        //         // create: function (e) {
        //         //     $.ajax({
        //         //         async: true,
        //         //         url: apiUrl + "api/talimatlar",
        //         //         method: 'POST',
        //         //         headers: {
        //         //             "content-type": "application/json",
        //         //             "cache-control": "no-cache"
        //         //         },
        //         //         type: "json",
        //         //         dataType: "json",
        //         //         data: JSON.stringify(e.data),
        //         //     }).success(function (data) {
        //         //         $("#talimatTuru").data("kendoGrid").dataSource.read();
        //         //     }).error(function (jqXHR, exception) {
        //         //         var errorJson = JSON.parse(jqXHR.responseText);
        //         //         genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
        //         //     }).always(function () {
        //         //         kendo.ui.progress($("#talimatTuru"), false);
        //         //     });
        //         // },
        update: function(e) {
          $.ajax({
            url: apiUrl + "api/talimatlar",
            method: "PUT",
            headers: {
              "content-type": "application/json",
              "cache-control": "no-cache"
            },
            type: "json",
            dataType: "json",
            data: JSON.stringify(e.data)
          })
            .success(function(data) {
              e.success();
              $("#talimatTuru")
                .data("kendoGrid")
                .dataSource.read();
            })
            .error(function(jqXHR, exception) {
              var errorJson = JSON.parse(jqXHR.responseText);
              genel.timer(
                300,
                'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");'
              );
            });
        },
        //         // destroy: function (e) {
        //         //     $.ajax({
        //         //         url: apiUrl + "api/talimatlar",
        //         //         method: 'DELETE',
        //         //         headers: {
        //         //             "content-type": "application/json",
        //         //             "cache-control": "no-cache"
        //         //         },
        //         //         type: "json",
        //         //         dataType: "json",
        //         //         data: JSON.stringify(e.data),
        //         //     }).success(function (data) {
        //         //         e.success();
        //         //         $("#talimatTuru").data("kendoGrid").dataSource.read();
        //         //     }).error(function (jqXHR, exception) {
        //         //         //console.log(exception);
        //         //         var errorJson = JSON.parse(jqXHR.responseText);
        //         //         genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
        //         //     });
        //         // }
        //     },
        //     template: {
        //     },
        //     schema: {
        //         model: {
        //             id: "talimatturu_id",
        //             fields: {
        //                 statu: { type: "boolean" },
        //                 varsayilan: { type: "boolean" },
        //                 kesim: { type: "boolean" },
        //                 dikim: { type: "boolean" },
        //                 parca: { type: "boolean" },
        //                 model: { type: "boolean" },
        //                 parcamodel_giris: { type: "boolean" },
        //                 parcamodel_cikis: { type: "boolean" },
        //                 model_zorunlu: { type: "boolean" },
        //                 varsayilan_fasoncu: { type: "number" }
        //             }
        //         }
        //     }
        // },
        toolbar: ["create"],
        editable: { mode: "inline", createAt: "bottom" }
      });
    }
  },
  sistemKaliteKontrolGet: {
    init: function() {
      genelayarlar.get.sistemKaliteKontrolGet.sistemKaliteKontrolGrid(
        "sistemKaliteKontrol"
      );
    },
    sistemKaliteKontrolGrid: function(element) {
      var columns = [];
      columns.push({
        title: "Sipariş Adedi",
        columns: [
          { title: "Alt Değer", field: "" },
          { title: "Üst Değer", field: "" }
        ]
      });
      columns.push({ title: "Kontrol Adedi", field: "" });
      columns.push({ title: "Max. Red Adedi", field: "" });
      columns.push({ title: "Değerler Oran", field: "" });
      columns.push({ title: "Ürün Grubu", field: "" });
      columns.push({
        command: [{ name: "edit" }, { name: "destroy" }],
        title: "İşlemler",
        width: "175px"
      });
      $("#sistemKaliteKontrol").kendoGrid({
        columns: columns,
        // dataSource: {
        //     transport: {
        //         read: function (e) {
        //             $.ajax({
        //                 url: apiUrl + "api/",
        //                 method: 'get',
        //                 headers: {
        //                     "content-type": "application/json",
        //                     "cache-control": "no-cache"
        //                 },
        //                 type: "json",
        //                 dataType: "json",
        //             }).success(function (data) {
        //                 console.log(data, "success")
        //                 e.success(data);
        //             }).error(function (jqXHR, exception) {
        //                 //console.log(exception);
        //                 var errorJson = JSON.parse(jqXHR.responseText);
        //                 genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
        //             });
        //         },
        //         // create: function (e) {
        //         //     $.ajax({
        //         //         async: true,
        //         //         url: apiUrl + "api/talimatlar",
        //         //         method: 'POST',
        //         //         headers: {
        //         //             "content-type": "application/json",
        //         //             "cache-control": "no-cache"
        //         //         },
        //         //         type: "json",
        //         //         dataType: "json",
        //         //         data: JSON.stringify(e.data),
        //         //     }).success(function (data) {
        //         //         $("#talimatTuru").data("kendoGrid").dataSource.read();
        //         //     }).error(function (jqXHR, exception) {
        //         //         var errorJson = JSON.parse(jqXHR.responseText);
        //         //         genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
        //         //     }).always(function () {
        //         //         kendo.ui.progress($("#talimatTuru"), false);
        //         //     });
        //         // },
        //         // update: function (e) {
        //         //     $.ajax({
        //         //         url: apiUrl + "api/talimatlar",
        //         //         method: 'PUT',
        //         //         headers: {
        //         //             "content-type": "application/json",
        //         //             "cache-control": "no-cache"
        //         //         },
        //         //         type: "json",
        //         //         dataType: "json",
        //         //         data: JSON.stringify(e.data),
        //         //     }).success(function (data) {
        //         //         e.success();
        //         //         $("#talimatTuru").data("kendoGrid").dataSource.read();
        //         //     }).error(function (jqXHR, exception) {
        //         //         var errorJson = JSON.parse(jqXHR.responseText);
        //         //         genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
        //         //     });
        //         // },
        //         // destroy: function (e) {
        //         //     $.ajax({
        //         //         url: apiUrl + "api/talimatlar",
        //         //         method: 'DELETE',
        //         //         headers: {
        //         //             "content-type": "application/json",
        //         //             "cache-control": "no-cache"
        //         //         },
        //         //         type: "json",
        //         //         dataType: "json",
        //         //         data: JSON.stringify(e.data),
        //         //     }).success(function (data) {
        //         //         e.success();
        //         //         $("#talimatTuru").data("kendoGrid").dataSource.read();
        //         //     }).error(function (jqXHR, exception) {
        //         //         //console.log(exception);
        //         //         var errorJson = JSON.parse(jqXHR.responseText);
        //         //         genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
        //         //     });
        //         // }
        //     },
        //     template: {
        //     },
        //     schema: {
        //         model: {
        //             id: "talimatturu_id",
        //             fields: {
        //                 statu: { type: "boolean" },
        //                 varsayilan: { type: "boolean" },
        //                 kesim: { type: "boolean" },
        //                 dikim: { type: "boolean" },
        //                 parca: { type: "boolean" },
        //                 model: { type: "boolean" },
        //                 parcamodel_giris: { type: "boolean" },
        //                 parcamodel_cikis: { type: "boolean" },
        //                 model_zorunlu: { type: "boolean" },
        //                 varsayilan_fasoncu: { type: "number" }
        //             }
        //         }
        //     }
        // },
        toolbar: ["create"],
        editable: { mode: "inline", createAt: "bottom" }
      });
    }
  }
};
genelayarlar.post = {};
genelayarlar.put = {};
genelayarlar.delete = {};

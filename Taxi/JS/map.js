var mapComponent = mapComponent || {};
 mapComponent = {

     options: {
         mapID: null,
         routeBut: null,
         pointAinside: null,
         pointBinside: null,
         priceFor1km: 5
     },
     myMap: null,
     init: function (options) {
         
         mapComponent.options = $.extend(mapComponent.options, options);
        
        
         mapComponent.myMap = new ymaps.Map(mapComponent.options.mapID, {
             center: [55.76, 37.64],
             zoom: 7
         });


         mapComponent.options.pointAinside.on("click", function (event) { mapComponent.options.pointAinside.val(""); });
         mapComponent.options.pointBinside.on("click", function (event) { mapComponent.options.pointBinside.val(""); });
         mapComponent.options.pointAinside.on("keyup", function (event) { mapComponent.findGeoMap(mapComponent.options.pointAinside, event) });
         mapComponent.options.pointBinside.on("keyup", function (event) { mapComponent.findGeoMap(mapComponent.options.pointBinside, event) })
         mapComponent.options.pointAinside.on("focusout", function () { mapComponent.checkForEmptyFieldsPoints(); });
         mapComponent.options.pointBinside.on("focusout", function () { mapComponent.checkForEmptyFieldsPoints(); });

         mapComponent.options.orderBut.on("click", function () { mapComponent.createNewOrder(); });
         mapComponent.options.sendOrderBut.on("click", function () { mapComponent.sendOrder(); });

     },

     checkForEmptyFieldsPoints: function () {

         var result = 0;

         if ((mapComponent.options.pointAinside.val() == "") || (mapComponent.options.pointBinside.val() == ""))
         {
             mapComponent.hideSendButtonAndClearKilometers();
             result = 1;
             return result;
         }
         else
         {
             mapComponent.options.alert_box.hide();
             return result;

         }

         

     },


     hideSendButtonAndClearKilometers: function(){
       //  mapComponent.options.orderBut.hide();
         $(".distance").html("");
         $(".price").html("");
         mapComponent.options.alert_box.show();

     },
  createRoute: function (pointAinside, pointBinside) {
      
         ymaps.route([pointAinside, pointBinside]).then(
    function (route) {
        mapComponent.myMap.geoObjects.add(route);
        $(".distance").html("Длина маршрута: "+ route.getHumanLength() + ".");
        $(".price").html("Стоимость поездки: " + parseInt(route.getHumanLength()) * mapComponent.options.priceFor1km + " р.");
        mapComponent.options.orderBut.show();
    },
    function (error) {

          mapComponent.options.modalError.modal("show");

        
    }
);


     },


  createNewOrder: function () {

      var result = mapComponent.checkForEmptyFieldsPoints();

      if (result == 1)
      {
          return false;
      }

         
         if (mapComponent.options.isAuthenticated == "true") {
         
             mapComponent.sendOrderAjaxBody(mapComponent.options.phoneNumber);
         }
         else
         {
           
             mapComponent.options.phoneNumberBox.mask("+7(999) 999-9999");
             
             mapComponent.options.modalSendOrder.modal("show");
         }
        

        
        
     },

 sendOrderAjaxBody: function (phoneNumber) {

         var jsonObject = {};

        

         jsonObject = {
             ID: 0,
             phone: phoneNumber,
             pointA: mapComponent.options.pointAinside.val(),
             pointB: mapComponent.options.pointBinside.val(),
             kilometers: parseInt($(".distance").text().replace(/\D/g, '')),
             price: parseInt($(".price").text().replace(/\D/g, '')),
             date: new Date(),
             status: "new",
             driver: null,
             dispatcher: null
         };



         var urlToSend = "/Home/addNewOrder";
         mapComponent.ajaxSend(urlToSend, jsonObject, function (data) { mapComponent.createdNewOrderCallback(data) }, mapComponent.options.sendOrderBut);



     },

     sendOrder: function(){
       
         if ((mapComponent.options.phoneNumberBox.val() == "") || (mapComponent.options.phoneNumberBox.val().length < 16))
         {
             
             mapComponent.options.modalSendOrder.find(".emptyPhone").show();
         }
         else
         {
             mapComponent.options.modalSendOrder.find(".emptyPhone").hide();


             mapComponent.sendOrderAjaxBody(mapComponent.options.phoneNumberBox.val());
             



         }

     },

     createdNewOrderCallback: function (data) {
         
         if (data.result == 1) {

            
             mapComponent.options.modalSendOrder.find(".sendOrderError").hide();
             mapComponent.options.phoneNumberBox.val("");
             mapComponent.options.modalSendOrder.modal("hide");
             $(".mainForm").html("<div class=\"alert alert-success\"><strong>Готово!</strong> Спасибо за то, что пользуетесь нашими услугами!</div><a href='/userCabinet/index' class='btn btn-info lookOrders'>Посмотреть заявки</a>");
            

         }
         else {

             if (mapComponent.options.isAuthenticated == "true")
             {
                 mapComponent.options.modalError.find(".modal-body").html("Возникла ошибка, отправьте заявку еще раз");
                 mapComponent.options.modalError.modal("show");
             }
             else
             {
                 mapComponent.options.modalSendOrder.find(".sendOrderError").show();
             }

             

         }
     },


     ajaxSend: function (url, data, callback, btn) {
         var params = data;
         var txt = "";
         if (btn) {
             btn.addClass('disabled');
             btn.attr('disabled', 'disabled');
         }
         
         var xhr = $.ajax({
             type: 'POST',
             url: url,
             cache: false,
             traditional: true,
             dataType: "json",
             contentType: 'application/json; charset=utf-8',
             data: JSON.stringify( params),
             success: function (data, status) {
                 var response = data;
                 if (data.d != undefined) response = data.d;
                 if (typeof (response) != "object") response = eval('(' + response + ')');
                 if (callback) callback(response);
             },
             complete: function () {
                 
                 if (btn) {
                     btn.removeClass('disabled');
                     btn.removeAttr('disabled');
                 }
             },
             error: function(jqXHR, textStatus, errorThrown){
                
             },
             beforeSend: function (xhr) {
                 xhr.setRequestHeader('Content-type', 'application/json; charset=utf-8');
             }
         });
         return xhr;
     },

 findGeoMap: function(obj, event) {
    
     active_suggestion = 0;

    
        
    var kc = parseInt(event.keyCode);
    if (kc == 40) {
        event.stopPropagation();
        var next =  + active_suggestion + 1;
        if (next >= $('.tx-suggestion').length) {
            next = 0;
        }
        $('.tx-suggestion').removeClass('tx-hover');
        $('.tx-suggestion:eq(' + next + ')').addClass('tx-hover');
        active_suggestion = next;
        return;
    } else if (kc == 38) {
        event.stopPropagation();
        var next =  + active_suggestion - 1;
        if (next < 0) {
            next = $('.tx-suggestion').length - 1;
        }
        $('.tx-suggestion').removeClass('tx-hover');
        $('.tx-suggestion:eq(' + next + ')').addClass('tx-hover');
        active_suggestion = next;
        return;
    }
    // if ($('#dropdown').css('display') != 'none') {
    if (kc == 13 && $('#dropdown .tx-suggestion.tx-hover').length > 0) {
        event.preventDefault();
        obj.val( $('.tx-suggestion.tx-hover a').text() );
        $('.tx-suggestion').removeClass('tt-hover');
        $('#dropdown').hide();
        active_suggestion = 0;
        return false;
    }
    if (kc == 27) {
        event.stopPropagation();
        $('.tx-suggestion').removeClass('tx-hover');
        $('#dropdown').hide();
        active_suggestion = 0;
        return;
    }
    // }
           
    var txt = $(obj).val();

            
    var ttt = txt.replace(/^(.)|\\s(.)/g, function ($1) { return $1.toUpperCase(); });
           
            
            
   
    $.ajax({
        url: 'https://geocode-maps.yandex.ru/1.x/?format=json&geocode=Россия,' + ttt,
        type:    'GET',
        success: function(data){
               

         
            lRussia = '';
            lOther  = '';
    
               

            if (data != '') {
                    
                   
                r = data;
                    
                html = '';
                
                for (a in r.response.GeoObjectCollection.featureMember) {
                    t = r.response.GeoObjectCollection.featureMember[a].GeoObject.metaDataProperty.GeocoderMetaData.text;
                    if (t.indexOf('Россия,') != -1) {

                        lRussia += '<div class="tx-suggestion tx-newcalc"><p><a href="#" title="">' + t + '</a></p></div>';

                    } else {
                        lOther += '<div class="tx-suggestion tx-newcalc"><p><a href="#" title="">' + t + '</a></p></div>';
                    }

                   
                    t = r.response.GeoObjectCollection.featureMember[a].GeoObject.metaDataProperty.GeocoderMetaData.text;
                  
                }


               
                $('#dropdown').html(lRussia + lOther);
              


                active_suggestion = 0;
                $('#dropdown .tx-suggestion:eq(0)').addClass('tx-hover');
 var positionTop = obj.position().top;
           
                var positionLeft = obj.position().left;

$('#dropdown').css("top", (obj.position().top + obj.height()+13) + "px");
               

                $('#dropdown').css("left", (obj.position().left) + "px");
             
                $('#dropdown').css("width", obj.width() + 28 + "px");
                $('#dropdown').show();


                mapComponent.checkGeoEvent(obj);


                // checkGeoEvent(obj.attr('rel'));
            }
        }
    });
 },
 checkGeoEvent: function(name){
        $('#dropdown a').click(function(){
            v = $(this).html();
            
            $(name).val(v);
            $('#dropdown').hide();


            if ((mapComponent.options.pointAinside.val() == "") || (mapComponent.options.pointBinside.val() == ""))
            {



            }
            else
            {
                mapComponent.createRoute(mapComponent.options.pointAinside.val(), mapComponent.options.pointBinside.val());
            }


            return false;
        });
}



}


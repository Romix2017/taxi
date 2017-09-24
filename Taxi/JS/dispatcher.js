var Dispatcher = Dispatcher || {}

Dispatcher = {

    options: {
        total: 0,
        assignObjectID: 0,
        assignedDriver: 0
    },

    init: function (options) {

        Dispatcher.options = $.extend(Dispatcher.options, options);

        Dispatcher.getTableViaAjax();

        Dispatcher.options.filterBlock.find("input[type=button]").on("click", function (event) {

            Dispatcher.setStatus(event);


        });
       

    },

    setStatus: function(event){

        Dispatcher.options.Status = $(event.target).data("status");
        Dispatcher.options.pageNumber = 1;
        Dispatcher.getTableViaAjax();


    },

    cancelOrder: function (event) {

      


        var button = event.target;


        Dispatcher.ajaxSend(Dispatcher.options.cancelLink, { orderID: event.target.getAttribute("data-id") }, function () { $("#cancelSuccess").modal("show"); Dispatcher.getTableViaAjax(); },null, $(button))
        


    },

    assignDriver: function(event)
    {
     

        Dispatcher.options.assignObjectID = event.target.getAttribute("data-id");
        var driverID = event.target.getAttribute("data-driver");

        Dispatcher.ajaxSend("/cms/getListOfDrivers", null, function (data, driverID) {   Dispatcher.getListOfDrivers(data, driverID);},driverID, null)
    },


    getListOfDrivers: function (data, driverID) {

       

       
        if (data.result == '1') {
          
            var str = [];

            str.push("<table class='driverTable'>");
            str.push("<tr><td>Водитель</td><td>Назначить</td></tr>");
            
            $.each(data.items, function (i, item) {
                var s = Dispatcher.getItemDriver(item, driverID);
                str.push("<tr>");
                str.push(s);
                str.push("</tr>");
               
            });
         
            str.push("</table>")
            $("#chooseDriver").find(".modal-body").html("");

          
            $("#chooseDriver").find(".modal-body").html(str.join(""));

            
            $("#chooseDriver").modal("show");
            
            $("input[type=radio]").on("change", function (event) {

                Dispatcher.setDriver(event);

            })

        }

        


    },


    setDriver: function (event) {

       var orderID = Dispatcher.options.assignObjectID;

       var driverID = event.target.id;


       Dispatcher.ajaxSend("/cms/setDriver", {orderID: orderID, driverID: driverID}, function (data) { Dispatcher.callbackSetDriver(data) },null, null)

    },

    callbackSetDriver: function (data) {

        

            $("#chooseDriver").modal("hide");

            Dispatcher.getTableViaAjax();
    


    },

    getItemDriver: function (item, driverID) {
        
        var s = [];

        s.push("<td>" + item.userName + "</td>");
        
        if (item.userID == driverID)
        {
           

            s.push("<td class='green_back'><div class='text-center'><input id='" + item.userID + "' type=\"radio\" name=\"optradio\" checked></div></td>");
        }
        else
        {
            s.push("<td><div class='text-center'><input id='" + item.userID + "' type=\"radio\" name=\"optradio\"></div></td>");
        }
        

       

        return s.join("");



    },

    getTableViaAjax: function () {

        var ajaxObj = {

            Status: Dispatcher.options.Status,
            pageSize: Dispatcher.options.pageSize,
            pageNumber: Dispatcher.options.pageNumber



        }


        Dispatcher.ajaxSend(Dispatcher.options.requestUrl, ajaxObj, function (data) { Dispatcher.ajaxSendCallback(data) },null, null);

    },


    initPagination: function (total) {
        $("#pagingcontainer").pagination(total, {
            items_per_page: Dispatcher.options.pageSize,
            callback: function (page_index, pagination_container) {
                Dispatcher.options.pageNumber = page_index + 1;
                Dispatcher.getTableViaAjax();
            },
            prev_text: "Назад",
            next_text: "Вперед",
            num_display_entries: 10,
            num_edge_entries: 1,
            current_page: Dispatcher.options.pageNumber - 1
        });
        var pagingcontainerwidth = 290 + 20 * total / Dispatcher.options.pageSize;
        $("#pagingcontainer").css('width', '' + pagingcontainerwidth + 'px');
    },

    ajaxSendCallback: function (data) {

        if (data.result == '1') {

            var str = [];

            str.push("<table>");
            str.push("<tr><td>Откуда</td><td>Куда</td><td>Расстояние</td><td>Цена</td><td>Дата</td><td>Статус</td><td>Отмена</td><td>Назначить</td></tr>");
            $.each(data.items, function (i, item) {
                var s = Dispatcher.getItem(item);
                if (item.driver == null)
                {
                    str.push("<tr>");
                }
                else
                {
                   
                    str.push("<tr class='assigned'>");
                }

               


                str.push(s);
                str.push("</tr>");
            });

            str.push("</table>")
            str.push("<div id=\"pagingwrapper\" class=\"pagingwrapper\">");
            str.push("<div id=\"pagingcontainer\" class=\"pagingcontainer\">");
            str.push("</div>");
            str.push("</div>");


            $('.tableOrders').html(str.join(""));
            Dispatcher.options.total = data.total;

            Dispatcher.initPagination(Dispatcher.options.total);
            $(".cancel").on("click", function (event) {  Dispatcher.cancelOrder(event) });
            $(".assign").on("click", function (event) {  Dispatcher.assignDriver(event) });

        }


    },

    getItem: function (item) {

        var s = [];
        var dateHere = new Date(parseInt(item.date.replace("/Date(", "").replace(")/", ""), 10));
        var statusBut = "<a href=\"#\" class=\"btn btn-default btn-circle\"><i class=\"fa fa-user\"></i></a>";

        switch (item.status) {
            case "new":
                statusBut = "<a href=\"#\" class=\"btn btn-warning btn-circle\" title=\"Выполняется\"><i class=\"fa fa-user\"></i></a>";
                break;
            case "canceled":
                statusBut = "<a href=\"#\" class=\"btn btn-danger btn-circle\" title=\"Отменена\"><i class=\"fa fa-user\"></i></a>";
                break;
            case "done":
                statusBut = "<a href=\"#\" class=\"btn btn-success btn-circle\" title=\"Выполнена\"><i class=\"fa fa-user\"></i></a>";
                break;
        }





        s.push("<td>" + item.pointA + "</td>");
        s.push("<td>" + item.pointB + "</td>");
        s.push("<td>" + item.kilometers + " км.</td>");
        s.push("<td>" + item.price + " р.</td>");
        s.push("<td>" + dateHere.toLocaleString() + "</td>");
        s.push("<td>" + statusBut + "</td>");

        if ((item.status == "canceled") || (item.status == "done"))
        {
            s.push("<td><a href='#' class='btn btn-danger cancel disabled' data-id='" + item.ID + "'>Отменить</a></td>");


            s.push("<td><a href='#' class='btn btn-success assign disabled' data-id='" + item.ID + "'>Назначить</a></td>");
        }
        else
        {
            s.push("<td><a href='#' class='btn btn-danger cancel' data-id='" + item.ID + "'>Отменить</a></td>");


            if (item.driver != null)
            {
                s.push("<td><a href='#' class='btn btn-success assign' data-driver='"+ item.driver+"' data-id='" + item.ID + "'>Назначить</a></td>");
            }
            else
            {
                s.push("<td><a href='#' class='btn btn-success assign' data-id='" + item.ID + "'>Назначить</a></td>");
            }

           



        }
        

        return s.join("");



    },
    ajaxSend: function (url, data, callback, secondParamForCallback, btn) {
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
            data: JSON.stringify(params),
            success: function (data, status) {

                var response = data;
                if (data.d != undefined) response = data.d;
                if (typeof (response) != "object") response = eval('(' + response + ')');
                if (callback) callback(response, secondParamForCallback);
            },
            complete: function () {

                if (btn) {
                    btn.removeClass('disabled');
                    btn.removeAttr('disabled');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

            },
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Content-type', 'application/json; charset=utf-8');
            }
        });
        return xhr;
    }















}
var Driver = Driver || {}

Driver = {

    options: {
        total: 0,
        assignObjectID: 0

    },

    init: function (options) {

        Driver.options = $.extend(Driver.options, options);

        Driver.getTableViaAjax();



    },
    getTableViaAjax: function () {
        
        var ajaxObj = {

            driverID: Driver.options.driverID,
            pageSize: Driver.options.pageSize,
            pageNumber: Driver.options.pageNumber


        }


        Driver.ajaxSend(Driver.options.requestUrl, ajaxObj, function (data) { Driver.ajaxSendCallback(data) }, null);

    },
    initPagination: function (total) {
        $("#pagingcontainer").pagination(total, {
            items_per_page: Driver.options.pageSize,
            callback: function (page_index, pagination_container) {
                Driver.options.pageNumber = page_index + 1;
                Driver.getTableViaAjax();
            },
            prev_text: "Назад",
            next_text: "Вперед",
            num_display_entries: 10,
            num_edge_entries: 1,
            current_page: Driver.options.pageNumber - 1
        });
        var pagingcontainerwidth = 252 + 20 * total / Driver.options.pageSize;
        $("#pagingcontainer").css('width', '' + pagingcontainerwidth + 'px');
    },

    ajaxSendCallback: function (data) {

        if (data.result == '1') {

            var str = [];

            str.push("<table>");
            str.push("<tr><td>Откуда</td><td>Куда</td><td>Расстояние</td><td>Цена</td><td>Дата</td><td>Статус</td><td>Отмена</td><td>Выполнена</td></tr>");
            $.each(data.items, function (i, item) {
                var s = Driver.getItem(item);
                if (item.driver == null) {
                    str.push("<tr>");
                }
                else {

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
            Driver.options.total = data.total;

            Driver.initPagination(Driver.options.total);
            $(".cancel").on("click", function (event) { Driver.cancelOrder(event) });
            $(".assign").on("click", function (event) { Driver.doneOrder(event) });

        }


    },
    cancelOrder: function (event) {




        var button = event.target;


        Driver.ajaxSend("/cms/cancelOrder", { orderID: event.target.getAttribute("data-id") }, function () { $("#changeSuccess").modal("show"); Driver.getTableViaAjax(); }, $(button))



    },

    doneOrder: function(event){
      
        var button = event.target;


        Driver.ajaxSend("/cms/doneOrder", { orderID: event.target.getAttribute("data-id") }, function () { $("#changeSuccess").modal("show"); Driver.getTableViaAjax(); }, $(button))

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

        if ((item.status == "canceled") || (item.status == "done")) {
            s.push("<td><a href='#' class='btn btn-danger cancel disabled' data-id='" + item.ID + "'>Отменить</a></td>");


            s.push("<td><a href='#' class='btn btn-success assign disabled' data-id='" + item.ID + "'>Выполнена</a></td>");
        }
        else {
            s.push("<td><a href='#' class='btn btn-danger cancel' data-id='" + item.ID + "'>Отменить</a></td>");


            s.push("<td><a href='#' class='btn btn-success assign' data-id='" + item.ID + "'>Выполнена</a></td>");
        }


        return s.join("");



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
            data: JSON.stringify(params),
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
            error: function (jqXHR, textStatus, errorThrown) {

            },
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Content-type', 'application/json; charset=utf-8');
            }
        });
        return xhr;
    }

}
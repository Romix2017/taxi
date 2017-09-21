var UserOrders = UserOrders || {}

UserOrders = {

    options: {
        total: 0
    },

    init: function (options) {

        UserOrders.options = $.extend(UserOrders.options, options);

        UserOrders.getTableViaAjax();
       

    },

    getTableViaAjax: function(){

        var ajaxObj = {

            UserPhone: UserOrders.options.UserPhone,
            pageSize: UserOrders.options.pageSize,
            pageNumber: UserOrders.options.pageNumber


        }


        UserOrders.ajaxSend(UserOrders.options.requestUrl, ajaxObj, function (data) { UserOrders.ajaxSendCallback(data) }, null);

    },


  initPagination:  function (total) {
            $("#pagingcontainer").pagination(total, {
items_per_page: UserOrders.options.pageSize,
callback: function (page_index, pagination_container) {
    UserOrders.options.pageNumber = page_index + 1;
    UserOrders.getTableViaAjax();
},
prev_text: "Назад",
    next_text: "Вперед",
num_display_entries: 10,
num_edge_entries: 1,
current_page: UserOrders.options.pageNumber - 1
});
var pagingcontainerwidth = 252 + 20 * total / UserOrders.options.pageSize;
$("#pagingcontainer").css('width', '' + pagingcontainerwidth + 'px');
},

    ajaxSendCallback: function(data)
    {
        
        if (data.result == '1') {
         
            var str = [];

            str.push("<table>");
            str.push("<tr><td>Откуда</td><td>Куда</td><td>Расстояние</td><td>Цена</td><td>Дата</td><td>Статус</td></tr>");
            $.each(data.items, function (i, item) {
                var s = UserOrders.getItem(item);
                str.push("<tr>");
                str.push(s);
                str.push("</tr>");
            });

            str.push("</table>")
            str.push("<div id=\"pagingwrapper\" class=\"pagingwrapper\">");
            str.push("<div id=\"pagingcontainer\" class=\"pagingcontainer\">");
            str.push("</div>");
            str.push("</div>");

            
            $('.tableOrders').html(str.join(""));
            UserOrders.options.total = data.total;
         
            UserOrders.initPagination(UserOrders.options.total);

        }


    },

        getItem: function(item)
        {
           
            var s = [];
            var dateHere = new Date(parseInt(item.date.replace("/Date(", "").replace(")/", ""), 10));
            var statusBut = "<a href=\"#\" class=\"btn btn-default btn-circle\"><i class=\"fa fa-user\"></i></a>";

            switch (item.status)
            {
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
            s.push("<td>" +dateHere.toLocaleString()+ "</td>");
            s.push("<td>" + statusBut + "</td>");

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
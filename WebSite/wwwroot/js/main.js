(function ($) {
    $.msg = function (msg) {
        layer.msg(msg);
    }
    $.alert = function (msg) {
        layer.alert(msg)
    }
    $.doPost = function (url, data, callBack) {
        var loading = layer.msg("提交中", { icon: 16, time: 9999999 });
        return $.post(url, data).done(callBack).error(function () { $.alert("提交异常") }).complete(function () { layer.close(loading) })
    }
    $.isWeiXin = function () {
        //window.navigator.userAgent属性包含了浏览器类型、版本、操作系统类型、浏览器引擎类型等信息，这个属性可以用来判断浏览器类型
        var ua = window.navigator.userAgent.toLowerCase();
        //通过正则表达式匹配ua中是否含有MicroMessenger字符串
        if (ua.match(/MicroMessenger/i) == 'micromessenger') {
            return true;
        } else {
            return false;
        }
    }
    $.client = function () {
        if (navigator.userAgent.match(/(iPhone|iPod|iPad);?/i)) { //判断是否是iOS
            return "ios";
        }
        if (navigator.userAgent.match(/android/i)) { //判断是否是Android
            return "android";
        }
    }
})($)
$(function () {
    $(".temp").click(function () {
        $.msg("即将上线，敬请期待！");
        return false;
    })
    $(".layloading").click(function () {
        var index = layer.load(1, {
            shade: [0.1, '#fff'] //0.1透明度的白色背景
        });
    })
    $('.datepicker').click(function () {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd', el: this, onpicked: function (dp) {
                if ("undefined" != typeof app && "undefined" != typeof Vue) {
                    var pickerValue = dp.cal.getNewDateStr();
                    var name = dp.el.getAttribute("name")
                    Vue.set(app.$data, name, pickerValue)
                }
            },
            firstDayOfWeek: 1
        })
    })
    $(".timepicker").click(function () {
        WdatePicker({
            dateFmt: 'HH:mm', el: this, onpicked: function (dp) {
                if ("undefined" != typeof app && "undefined" != typeof Vue) {
                    var pickerValue = dp.cal.getNewDateStr();
                    var name = dp.el.getAttribute("name")
                    Vue.set(app.$data, name, pickerValue)
                }
            }

        })
    })
    $('.datetimepicker').click(function () {
        WdatePicker({
            dateFmt: 'yyyy-MM-dd HH:mm', el: this, onpicked: function (dp) {
                if ("undefined" != typeof app && "undefined" != typeof Vue) {
                    var pickerValue = dp.cal.getNewDateStr();
                    var name = dp.el.getAttribute("name")
                    Vue.set(app.$data, name, pickerValue)
                }
            },
            firstDayOfWeek: 1
        })
    })
    $(".paginate_button:not(.disabled):not(.current)").click(function () {
        var load = layer.msg('提交中...', { icon: 16, time: 36000, shade: 0.3 });
    })
    $("input[name='FromCity'],input[name='ToCity'],input[name='Carrier'],input[name='Cabin']").keyup(function () {
        $(this).val($(this).val().toUpperCase())
    })
})
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}
/**
 * *******************************************************************************
 *   校验插件
 * *******************************************************************************
 */
function Verifydata(cname) {
    var $cell = $(cname);
    for (var i = 0; i < $cell.length; i++) {
        if ($($cell[i]).find(".el-select") != undefined) {
            var verify = $($cell[i]).find(".el-select").attr("verify");
            if (verify != undefined && verify != "" && verify != null) {
                $($cell[i]).find("input").attr("verify", verify);
                $($cell[i]).find(".el-select").attr("verify", "");
            }
        }
        if ($($cell[i]).find(".el-date-editor") != undefined) {
            var verify = $($cell[i]).find(".el-date-editor").attr("verify");
            if (verify != undefined && verify != "" && verify != null) {
                $($cell[i]).find("input").attr("verify", verify);
                $($cell[i]).find(".el-date-editor").attr("verify", "");
            }
        }
    }
    $(cname).find("input").off("mouseover");
    $(cname).find("input").off("blur");
    $(cname).find("input").on('mouseover', function () {
        Verification(this);
        mouseover(this);
    }).on('blur', function () {
        Verification(this);
        mouseout(this);
    });
    $(".el-select__tags").off("mouseover");
    $(".el-select__tags").on('mouseover', function () {
        Verification(this);
        mouseover(this);
    })
}

function Verification(elem) {
    var value = $(elem).val(), msg = "", cname = "";
    var mode = $(elem).attr("verify");
    var $contrl = $(elem).parent();
    for (var i = 1; i < 6; i++) {
        cname = "class:" + $contrl.attr("class");
        if (cname.indexOf("cell") < 1) {
            if (cname.indexOf("el-form-item__content") < 1) {
                $contrl = $contrl.parent();
            } else {
                $contrl = $contrl.find(".el-input");
            }
        }
    }
    if (mode != null && mode != "" && mode != undefined) {
        $contrl.removeClass("error");
        mode = mode.replace("]", "");
        if (mode.indexOf("Notempty") > 0) {
            if (value == "" || value == undefined) {
                msg = "此处不能为空!";
            }
        }
        if (mode.indexOf("SelectEmpty") > 0) {
            if (value == "" || value == undefined) {
                var val = $contrl.find(".el-select__tags").find("span").length > 0 ? "1" : "";
                if (val == "") {
                    msg = "此处不能为空!";
                }
            }
        }
        if (mode.indexOf("Number") > 0) {
            if (!(/^[0-9.]*$/).test(value)) {
                msg = "请填写正确的数字!";
            }
        }
        if (mode.indexOf("minValue") > 0) {
            var num = 0;
            var arr = mode.split(',');
            for (var i = 0; i < arr.length; i++) {
                var ops = "opt-" + arr[i];
                if (ops.indexOf("minValue") > 0) {
                    num = parseFloat(ops.split(':')[1]);
                }
            }
            if (parseFloat(value) < num) {
                msg = "请填写大于" + value + "的数字!";
            }
        }
        if (mode.indexOf("maxValue") > 0) {
            var num = 0;
            var arr = mode.substring(1, mode.length - 2).split(',');
            for (var i = 0; i < arr.length; i++) {
                if (("opt-" + arr[i]).indexOf("maxValue") > 0) {
                    num = parseFloat(arr[i].split(':')[1]);
                }
            }
            if (parseFloat(value) > num) {
                msg = "请填写小于" + value + "的数字!";
            }
        }
        if (mode.indexOf("minLength") > 0) {
            var num = 0;
            var arr = mode.substring(1, mode.length - 2).split(',');
            for (var i = 0; i < arr.length; i++) {
                if (("opt-" + arr[i]).indexOf("minLength") > 0) {
                    num = parseFloat(arr[i].split(':')[1]);
                }
            }
            if (!value.length > num) {
                msg = "请填写长度大于" + value + "的值!";
            }
        }
        if (mode.indexOf("maxLength") > 0) {
            var num = 0;
            var arr = mode.substring(1, mode.length - 2).split(',');
            for (var i = 0; i < arr.length; i++) {
                if (("opt-" + arr[i]).indexOf("maxLength") > 0) {
                    num = parseFloat(arr[i].split(':')[1]);
                }
            }
            if (!value.length < num) {
                msg = "请填写长度小于" + value + "的值!";
            }
        }
    }
    if (msg != "") {
        $contrl.addClass("error");
        $(elem).attr("error", msg);
    } else {
        $(elem).attr("error", "");
    }
    return msg;
}

function mouseover(elem) {
    var error = $(elem).attr("error");
    if (error != undefined && error != null && error != "") {
        //layer.tips(error, elem, { tips: [1, '#fff'], time: 3000, skin: 'ErrorTips' });
    }
}

function mouseout(elem) {
    var error = $(elem).attr("error");
    if (error != undefined && error != null && error != "") {
        //layer.close(layer.index);
    }
}

Vue.prototype.Verifydata = function (cname) {
    var $cell = $(cname);
    for (var i = 0; i < $cell.length; i++) {
        if ($($cell[i]).find(".el-select") != undefined) {
            var verify = $($cell[i]).find(".el-select").attr("verify");
            if (verify != undefined && verify != "" && verify != null) {
                $($cell[i]).find("input").attr("verify", verify);
                $($cell[i]).find(".el-select").attr("verify", "");
            }
        }
        if ($($cell[i]).find(".el-date-editor") != undefined) {
            var verify = $($cell[i]).find(".el-date-editor").attr("verify");
            if (verify != undefined && verify != "" && verify != null) {
                $($cell[i]).find("input").attr("verify", verify);
                $($cell[i]).find(".el-date-editor").attr("verify", "");
            }
        }
    }
    $(cname).find("input").off("mouseover");
    $(cname).find("input").off("blur");
    $(cname).find("input").on('mouseover', function () {
        Verification(this);
        mouseover(this);
    }).on('blur', function () {
        Verification(this);
        mouseout(this);
    });
    $(".el-select__tags").off("mouseover");
    $(".el-select__tags").on('mouseover', function () {
        Verification(this);
        mouseover(this);
    })
}

Vue.prototype.Verification = function (elem) {
    var value = $(elem).val(), msg = "", cname = "";
    var mode = $(elem).attr("verify");
    var $contrl = $(elem).parent();
    for (var i = 1; i < 6; i++) {
        cname = "class:" + $contrl.attr("class");
        if (cname.indexOf("cell") < 1) {
            if (cname.indexOf("el-form-item__content") < 1) {
                $contrl = $contrl.parent();
            } else {
                $contrl = $contrl.find(".el-input");
            }
        }
    }
    if (mode != null && mode != "" && mode != undefined) {
        $contrl.removeClass("error");
        mode = mode.replace("]", "");
        if (mode.indexOf("Notempty") > 0) {
            if (value == "" || value == undefined) {
                msg = "此处不能为空!";
            }
        }
        if (mode.indexOf("SelectEmpty") > 0) {
            if (value == "" || value == undefined) {
                var val = $contrl.find(".el-select__tags").find("span").length > 0 ? "1" : "";
                if (val == "") {
                    msg = "此处不能为空!";
                }
            }
        }
        if (mode.indexOf("Number") > 0) {
            if (!(/^[0-9.]*$/).test(value)) {
                msg = "请填写正确的数字!";
            }
        }
        if (mode.indexOf("minValue") > 0) {
            var num = 0;
            var arr = mode.split(',');
            for (var i = 0; i < arr.length; i++) {
                var ops = "opt-" + arr[i];
                if (ops.indexOf("minValue") > 0) {
                    num = parseFloat(ops.split(':')[1]);
                }
            }
            if (parseFloat(value) < num) {
                msg = "请填写大于" + value + "的数字!";
            }
        }
        if (mode.indexOf("maxValue") > 0) {
            var num = 0;
            var arr = mode.substring(1, mode.length - 2).split(',');
            for (var i = 0; i < arr.length; i++) {
                if (("opt-" + arr[i]).indexOf("maxValue") > 0) {
                    num = parseFloat(arr[i].split(':')[1]);
                }
            }
            if (parseFloat(value) > num) {
                msg = "请填写小于" + value + "的数字!";
            }
        }
        if (mode.indexOf("minLength") > 0) {
            var num = 0;
            var arr = mode.substring(1, mode.length - 2).split(',');
            for (var i = 0; i < arr.length; i++) {
                if (("opt-" + arr[i]).indexOf("minLength") > 0) {
                    num = parseFloat(arr[i].split(':')[1]);
                }
            }
            if (!value.length > num) {
                msg = "请填写长度大于" + value + "的值!";
            }
        }
        if (mode.indexOf("maxLength") > 0) {
            var num = 0;
            var arr = mode.substring(1, mode.length - 2).split(',');
            for (var i = 0; i < arr.length; i++) {
                if (("opt-" + arr[i]).indexOf("maxLength") > 0) {
                    num = parseFloat(arr[i].split(':')[1]);
                }
            }
            if (!value.length < num) {
                msg = "请填写长度小于" + value + "的值!";
            }
        }
    }
    if (msg != "") {
        $contrl.addClass("error");
        $(elem).attr("error", msg);
    } else {
        $(elem).attr("error", "");
    }
    return msg;
}

Vue.prototype.mouseover = function (elem) {
    var error = $(elem).attr("error");
    if (error != undefined && error != null && error != "") {
        //layer.tips(error, elem, { tips: [1, '#fff'], time: 3000, skin: 'ErrorTips' });
    }
}

Vue.prototype.mouseout = function (elem) {
    var error = $(elem).attr("error");
    if (error != undefined && error != null && error != "") {
        //layer.close(layer.index);
    }
}

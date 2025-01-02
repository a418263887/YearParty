
$(function () {
    var $cell = $(".cell");
    for (var i = 0; i < $cell.length; i++) {
        if ($($cell[i]).find(".el-select") != undefined) {
            var verify = $($cell[i]).find(".el-select").attr("verify");
            if (verify != undefined && verify != "" && verify != null) {
                $($cell[i]).find("input").attr("verify", verify);
                $($cell[i]).find(".el-select").attr("verify", "");
            }
        }

        if ($($cell[i]).find(".el-input-number") != undefined) {
            var verify = $($cell[i]).find(".el-input-number").attr("verify");
            if (verify != undefined && verify != "" && verify != null) {
                $($cell[i]).find("input").attr("verify", verify);
                $($cell[i]).find(".el-input-number").attr("verify", "");
            }
        }
    }
    //var $cell = $(".el-form-item__content");
    //for (var i = 0; i < $cell.length; i++) {
    //    if ($($cell[i]).find(".el-select") != undefined) {
    //        var verify = $($cell[i]).find(".el-select").attr("verify");
    //        if (verify != undefined && verify != "" && verify != null) {
    //            $($cell[i]).find("input").attr("verify", verify);
    //            $($cell[i]).find(".el-select").attr("verify", "");
    //        }
    //    }

    //}
    var $cell = $(".el-descriptions-item__content");
    for (var i = 0; i < $cell.length; i++) {




        if ($($cell[i]).find(".el-select") != undefined) {
            var verify = $($cell[i]).find(".el-select").attr("verify");
            if (verify != undefined && verify != "" && verify != null) {
                $($cell[i]).find("input").attr("verify", verify);
                $($cell[i]).find(".el-select").attr("verify", "");
            }
        }

    }
    //Verifydata(".cell");
    //Verifydata(".el-form-item__content");
})


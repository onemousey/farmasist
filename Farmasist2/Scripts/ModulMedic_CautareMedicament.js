var ajaxHelper = function () {
    function get(url, successCallback, failureCallBack) {
        $.ajax({
            url: url,
            dataType: "json",
            type: "GET",
            contentType: 'application/json; charset=utf-8',
            async: true,
            processData: false,
            cache: false,
            success: successCallback,
            error: failureCallBack
        });
    }

    function post(url, postData, successCallback, failureCallBack) {
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(postData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: successCallback,
            failure: failureCallBack
        });
    }
    function postPdf(url, postData, successCallback, failureCallBack) {
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(postData),
            contentType: "application/pdf",
            success: successCallback,
            failure: failureCallBack
        });
    }
    return {
        get: get,
        post: post,
        postPdf: postPdf
    }
}();

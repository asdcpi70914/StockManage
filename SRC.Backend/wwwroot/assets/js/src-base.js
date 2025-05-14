

//function tprDownload(data,filename) {
//    let tmp = document.createElement('a');
//    var blob = new Blob([data], { type: 'application/unknow' });
//    let objUrl = window.URL.createObjectURL(blob);
//    tmp.href = objUrl;
//    tmp.download = filename;//(accountNumber + "_cert.oec");
//    tmp.click();
//    window.URL.revokeObjectURL(objUrl);
//}


    function _srcvalley_pg_loading(id) {
        //$.blockUI.defaults = {
        //    fadeIn: 1,
        //    fadeOut: 1,
        //};

        //$.blockUI.defaults.fadeIn = 1;
        //$.blockUI.defaults.fadeOut = 200;

        //console.log(id)

        if (id === undefined || id === null) {
        //console.log(id)
        id = "#_loadingpage";
            //console.log(id)
        } else {
        id = "#" + id;
        }

        //console.log(id)
        //$.blockUI({message: $('#_loadingpage') });
    $("#_loadingpage").removeClass('d-none');
    $.blockUI({
        //message: $(id),
        message: '<div class="d-flex justify-content-center">' +
            '<div class="">' +
                '<div class="ball-scale-ripple-multiple" style="margin-left:25px">'+
                    '<div></div>'+
                '<div></div>'+
                '<div></div>'+
                '<div></div>'+
                '<div></div>'+
                '</div>'+
            '<h5 class="text-white" style="margin-top:30px">處理中</h5>'+
            '</div>'+
        '</div>',
    baseZ : 2147483647,
    css: {
        border: 'none',
        padding: '36px 0px 9px 0px',
    backgroundColor: '#000',
    '-webkit-border-radius': '10px',
    '-moz-border-radius': '10px',
        opacity: 1,
        width: '15%',
        left:'45%',
        //message:"處理中..."
                //color: 
            }
        });
    }

    function _srcvalley_pg_loading_stop() {
        //$(document).ajaxStop($.unblockUI);

        $.unblockUI();
    }


function tprDownloadForBase64File(data) {
    let tmp = document.createElement('a');
    //console.log(data.data);

    let decodeb64 = window.atob(data.data);
    let byteArray = new Uint8Array(decodeb64.length);

    for (i = 0; i < byteArray.length; i++) {
        byteArray[i] = decodeb64.charCodeAt(i);
    }

    let blob = new Blob([byteArray], { type: 'application/octet-stream' });
    let objUrl = window.URL.createObjectURL(blob);
    tmp.href = objUrl;
    tmp.download = data.fileName;
    tmp.click();

    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveOrOpenBlob(blob, data.fileName);
    } else {
        window.URL.revokeObjectURL(objUrl);
    }
}

function _src_createOuterDivModal(data) {

    if ( $("#_src_tmp_outerdivmodal").length >= 1)
    {
        $("#_src_tmp_outerdivmodal").remove();
    }

    let tmpDiv = document.createElement("div");
    tmpDiv.setAttribute("id", "_src_tmp_outerdivmodal");
    tmpDiv.insertAdjacentHTML('beforeend', data);
    $("body").append(tmpDiv);


}


function _src_select_option_exchange_(fromId, toId , isSelected) {

    let obj = {
        val: $("#" + fromId).val(),
        txt: $("#" + fromId + " option:selected").html()
    };

    if (obj.txt === undefined) {
        return;
    }

    $("#" + fromId + " option:selected").remove();

    let opt = new Option(obj.txt, obj.val);
    opt.selected = isSelected;
    $("#" + toId).append(opt);

    return obj;

}

function _src_select_option_moveto(fromId , toId) {
    $("#" + fromId+" option").each(function () {
        let opt = new Option($(this).html(), $(this).val());
        $("#" + toId).append(opt);
    });
    $("#" + fromId+" option").remove();
}

//function _src_popimg(imgname) {

//    let md = "<div id='__src_pop_img_m' class='modal' style='z-index: 10000'>";
//    md += "<span class='close'>&times;</span>";
//    md += "<img class='modal-content' id='_src_pop_img_target'>";
//    md += "<div id='caption'></div></div>";

//    // Get the modal
//    var modal = document.getElementById("__src_pop_img_m");
//    $("#" + imgname).append(md);

//    // Get the image and insert it inside the modal - use its "alt" text as a caption
//    var img = document.getElementById(imgname);
//    var modalImg = document.getElementById("_src_pop_img_target");
//    //var captionText = document.getElementById("caption");
//    img.onclick = function () {
//        modal.style.display = "block";
//        modalImg.src = this.src;
//        //captionText.innerHTML = this.alt;
//    }

//    // Get the <span> element that closes the modal
//    //var span = document.getElementsByClassName("close")[0];

//    // When the user clicks on <span> (x), close the modal

//    $("#__src_pop_img_m .close").on('click', function () {
//        console.log("A");
//        modal.style.display = "none";
//        $("#__src_pop_img_m").remove();
//    });


//}

function _src_bindtools_click(id, bindfun, successfun, failfun) {
    $.each($("[id^='" + id + "']"), function () {
        $(this).on('click', function () {

            if (typeof bindfun !== 'function') {
                console.log("parameter mainfun is not a function")
                return;
            }

            bindfun(this).then(
                success => {
                    if (successfun != null) {
                        if (typeof successfun === 'function') {
                            successfun();
                        } else {
                            console.log("parameter successfun is not a function");
                        }
                    }
                },
                fail => {
                    if (failfun != null) {
                        if (typeof failfun === 'function') {
                            failfun();
                        } else {
                            console.log("parameter failfun is not a function");
                        }
                    }
                }
            );

        });

    });
}


function _src_promise_ajax(
    type,
    url,
    data,
    successfun) {

    if (typeof successfun === 'function') {
        let p1 = new Promise((resolve, reject) => {
            $.ajax({
                type: type,
                url: url,
                data: data,
                dataType: "html",
                success: function (result) {
                    successfun(result);
                    resolve(true);
                },
                beforeSend: function () {
                    _srcvalley_pg_loading();
                },
                complete: function () {
                    _srcvalley_pg_loading_stop();
                }

            });

        });

        return p1;
    }

    if (successfun != null) {
        console.log("parameter promisefun is not a function");
    }
    

    let p2 = new Promise((resolve, reject) => { resolve(true); });

    return p2;
}


function _src_formdata(formid) {

    let uidata = new FormData();

    $("#" + formid+" input[type='file']").each(function () {
        let fi = $(this).get(0).files;
        if (fi.length > 0) {
            uidata.append($(this).attr("name"), fi[0]);
        }
    });

    $("#" + formid +" input[type='text']").each(function () {
        uidata.append($(this).attr("name"), $(this).val());
    });

    $("#" + formid +" input[type='hidden']").each(function () {
        uidata.append($(this).attr("name"), $(this).val());
    });

    $("#" + formid ).find(":checkbox:checked, :radio:checked").each(function () {
        uidata.append(this.name, $(this).val());
    });


    return uidata;
    
}


function _b64toBlob(b64) {
    var byteString = atob(b64.split(',')[1]);

    var mimeString = b64.split(',')[0].split(':')[1].split(';')[0];

    var ab = new ArrayBuffer(byteString.length);
    var ia = new Uint8Array(ab);
    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }

    return new Blob([ab], { type: mimeString });
}

function _blobToBase64(blob) {
    return new Promise((resolve, _) => {
        const reader = new FileReader();
        reader.onloadend = () => resolve(reader.result);
        reader.readAsDataURL(blob);
    });
}
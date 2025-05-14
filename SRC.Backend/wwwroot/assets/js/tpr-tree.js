$.fn.extend({
    checkAll : function (checked){
        $('.tpr-tree input[type=checkbox]').prop('indeterminate', false);
        $('.tpr-tree input[type=checkbox]').prop('checked', checked);
    },
    /* 向上檢查 */
    checkParent: function (parentId) {


        //console.log("p-" + parentId);

        var indeterminate = $('input[parent='+parentId+']:indeterminate').length;
         
        var checked = $('input[parent="'+parentId+'"]:checked').length;
        var total = $('input[parent="'+parentId+'"]').length;

        //console.log("chk-" + checked);
        //console.log("total-" + checked);
        //console.log("indeterminate-" + checked);

        if(checked == total){
            $('#' + parentId).prop('indeterminate', false);
            $('#' + parentId).prop('checked', true);
        }else if(indeterminate != 0 ){
            $('#' + parentId).prop('indeterminate', true);
        }else if(checked == 0 ){
            $('#' + parentId).prop('checked', false);
            $('#' + parentId).prop('indeterminate', false);
            
        }else if($('input[parent="'+parentId+'"]:checked').length>0){
            $('#' + parentId).prop('indeterminate', true);
            /* after indeterminate */
            $('#' + parentId).prop('checked', true);
        }else{
            $('#' + parentId).prop('indeterminate', false);
            
        }

       if($('#' + parentId).attr('hasParent')){
           var p = $('#' + parentId).attr('parent');
           $(this).checkParent(p);
           if ($('#' + parentId).prop('indeterminate')) {
               $('#' + p).prop('checked', true);
               $('#' + p).prop('indeterminate',true);
           }
           
       }else{
           //$('#' + p).prop('indeterminate',false);
       }
        
        
    },
    /* 向下檢查 */
    checkChild : function(parentId , checked){   
        $('input[parent=' + parentId + ']').prop('indeterminate', false);
        $('input[parent=' + parentId + ']').prop("checked", checked);

        var childs = $('input[parent=' + parentId + ']');
        
        $.each(childs , function(index,item){
          var parentId = $(this).attr('id');
          if(parentId){              
            $(this).checkChild(parentId , checked);
          }
        });
    },
    /* 切換 */
    treeToggle : function(action){
        var icon = $('.tpr-tree-icon');
        var childDiv = $('.tpr-tree-div');
        if('open' == action){
            icon.addClass('tpr-tree-icon-rotate');    
            childDiv.show(100);
        } else {
            icon.removeClass('tpr-tree-icon-rotate');
            childDiv.hide(100);
        }
    },
    /* 取值 */
    getChecked : function(action){
        var checked = [];
        var indeterminate = [];
        var res = $('input[class^="tpr-tree-ck"]:checked');
        $.each(res , function(index,item){   
            var pid =  $(this).attr('pid');
            checked.push(pid);
        });
         
        res = $('input[class^="tpr-tree-ck"]:indeterminate');
        $.each(res , function(index,item){
           var pid =  $(this).attr('pid');
           indeterminate.push(pid);
        });
        var ret = {'checked':checked,'indeterminate':indeterminate};
       
        return ret;
    },
    /* tree */
    tprTree: function (action) {
        /* icon 切換 */
        $('.tpr-tree-span').on('click', function () {
            var icon = $(this).children( "i" );
            var parentId = $(this).attr("id");
            if(parentId){
                var childId = parentId.replace('-p-' , '-c-');        
                if( icon.hasClass('tpr-tree-icon-rotate') ){
                    icon.removeClass('tpr-tree-icon-rotate');
                    $('.'+childId).hide(100);
                }else{
                    icon.addClass('tpr-tree-icon-rotate');    
                    $('.'+childId).show(100);
                }
            }
        });
        
        /* 收合 */
        $('.tpr-tree-toggle').on('click',function(){
            if( $('.tpr-tree-icon-rotate').length > 0){
                $(this).treeToggle('close')
            }else{
                $(this).treeToggle('open');
            }
        });
        /* 展開 */
        $('.tpr-tree-open').on('click',function(){
            $(this).treeToggle('open');
        });
        /* 收起 */
        $('.tpr-tree-close').on('click',function(){
            $(this).treeToggle('close');
        });
         
        /* 全選 */
        $('.tpr-tree-checkAll').on('click',function(){
            $(this).checkAll(true);
        });
        /* 全不選 */
        $('.tpr-tree-uncheckAll').on('click',function(){
            $(this).checkAll(false);
        });
        
        /* 檢查 */
        $('input[class^="tpr-tree-ck"]').on('click',function(){

            /* 向上 */
            if($(this).attr('hasParent')) {
                $(this).checkParent($(this).attr('parent'));
            }
            
            /* 向下 */
            if($(this).attr('hasChild')){
                var parentId = $(this).attr("id");
                var checked = $(this).prop("checked");
                $(this).checkChild(parentId , checked);
            }
           
       
        });
        
        // FOR TEST 回傳值
        $('.tpr-tree-chked').on('click' ,function(){
            var res = $('input[class^="tpr-tree-ck"]:checked');
            
            $('.result').html('');
            $('.result').append( '-- checked --<br>' );
            $.each(res , function(index,item){
                var txt = document.createElement("p");
                var pid =  $(this).attr('pid');
                //txt.append(pid);
                //$('.result').append(txt);
                $('.result').append('<p>' + pid + '</p>');
            });
            $('.result').append( '-- indeterminate --' );
            res = $('input[class^="tpr-tree-ck"]:indeterminate');
            $.each(res , function(index,item){
                var txt = document.createElement("p");
                var pid =  $(this).attr('pid');
                //txt.append(pid);
                //$('.result').append(txt);
                $('.result').append('<p>' + pid + '</p>');
            });
            
        } );
        
        /* 初始化選擇 */
        var parents = $('input[hasChild="true"]');
        $.each(parents , function(index,item){
            var parentId = $(this).attr("id");
            var checked = $(this).prop("checked");
            if(checked){
                $(this).checkChild(parentId , checked);
            }
            
        });
        
        var childs = $('input[hasParent="true"]');
        $.each(childs , function(index,item){
            $(this).checkParent($(this).attr('parent'));
        });
        
        
        /* 初始化開合 */
        var childDiv = $('.tpr-tree-div');
        if (action == 'show') {
            $('.tpr-tree-span').click(); 
            childDiv.show();
        }else{
            childDiv.hide();
        }
        
        return this;
    }
});


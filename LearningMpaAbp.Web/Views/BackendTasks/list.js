$(function() {

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();

});

var taskService = abp.services.app.task;
var $table = $('#tb-tasks');
var TableInit = function() {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function() {
        $table.bootstrapTable({
            url: '/BackendTasks/GetAllTasks', //请求后台的URL（*）
            method: 'get', //请求方式（*）
            toolbar: '#toolbar', //工具按钮用哪个容器
            striped: true, //是否显示行间隔色
            cache: false, //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true, //是否显示分页（*）
            sortable: true, //是否启用排序
            sortOrder: "asc", //排序方式
            queryParams: oTableInit.queryParams, //传递参数（*）
            sidePagination: "server", //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1, //初始化加载第一页，默认第一页
            pageSize: 5, //每页的记录行数（*）
            pageList: [10, 25, 50, 100], //可供选择的每页的行数（*）
            search: false, //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true, //是否显示所有的列
            showRefresh: true, //是否显示刷新按钮
            minimumCountColumns: 2, //最少允许的列数
            clickToSelect: true, //是否启用点击选中行
            height: 500, //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "Id", //每一行的唯一标识，一般为主键列
            showToggle: true, //是否显示详细视图和列表视图的切换按钮
            cardView: false, //是否显示详细视图
            detailView: false, //是否显示父子表
            columns: [
                {
                    radio: true
                }, {
                    field: 'Title',
                    title: '任务名称',
                    sortable: true
                }, {
                    field: 'Description',
                    title: '任务描述'
                }, {
                    field: 'AssignedPersonName',
                    title: '任务分配'
                }, {
                    field: 'State',
                    title: '任务状态',
                    formatter: showState
                }, {
                    field: 'CreationTime',
                    title: '创建日期',
                    formatter: showDate
                }, {
                    field: 'operate',
                    title: '操作',
                    align: 'center',
                    valign: 'middle',
                    clickToSelect: false,
                    formatter: operateFormatter,
                    events: operateEvents
                }
            ]
        });
    };

    //指定操作组
    function operateFormatter(value, row, index) {
        return [
            '<a class="like" href="javascript:void(0)" title="Like">',
            '<i class="glyphicon glyphicon-heart"></i>',
            '</a>',
            ' <a class="edit" href="javascript:void(0)" title="Edit">',
            '<i class="glyphicon glyphicon-edit"></i>',
            '</a>',
            ' <a class="remove" href="javascript:void(0)" title="Remove">',
            '<i class="glyphicon glyphicon-remove"></i>',
            '</a>'
        ].join('');
    }

    //指定table表体操作事件
    window.operateEvents = {
        'click .like': function(e, value, row, index) {
            alert('You click like icon, row: ' + JSON.stringify(row));
            console.log(value, row, index);
        },
        'click .edit': function(e, value, row, index) {
            //alert('You click edit icon, row: ' + JSON.stringify(row));
            //console.log(value, row, index);

            editTask(row.Id);
        },
        'click .remove': function(e, value, row, index) {
            //alert('You click remove icon, row: ' + JSON.stringify(row));
            //console.log(value, row, index);

            deleteTask(row.Id);
        }
    };

    //指定bootstrap-table查询参数
    oTableInit.queryParams = function(params) {
        var temp = { //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit, //页面大小
            offset: params.offset, //页码
            sortfiled: params.sort, //排序字段
            sortway: params.order, //升序降序
            search: $("#txt-filter").val(), //自定义传参-任务名称
            status: $("#txt-search-status").val() //自定义传参-任务状态
        };
        return temp;
    };

    //格式化显示json日期格式
    function showDate(value, row, index) {
        var birthdayMilliseconds = parseInt(value.replace(/\D/igm, ""));
        var date = new Date(birthdayMilliseconds);
        var formatDate = date.toLocaleString();
        return formatDate;
    }

    //格式化显示任务状态
    //有待改进-获取任务状态列表
    function showState(value, row, index) {
        var formatState;
        if (value === 0) {
            formatState = '<span class="pull-right label label-success">Open</span>';
        }
        if (value === 1) {
            formatState = '<span class="pull-right label label-info">Completed</span>';
        }

        return formatState;
    }

    return oTableInit;
};


//bootstrap-table工具栏按钮事件初始化
var ButtonInit = function() {
    var oInit = new Object();
    var postdata = {};

    oInit.Init = function() {
        //初始化页面上面的按钮事件
        $("#btn-add")
            .click(function() {
                $("#add").modal("show");
            });

        $("#btn-edit")
            .click(function() {
                var selectedRaido = $table.bootstrapTable('getSelections');
                if (selectedRaido.length === 0) {
                    abp.notify.warn("请先选择要编辑的行！");
                } else {
                    editTask(selectedRaido[0].Id);
                }
            });

        $("#btn-delete")
            .click(function() {
                var selectedRaido = $table.bootstrapTable('getSelections');
                if (selectedRaido.length === 0) {
                    abp.notify.warn("请先选择要删除的行！");
                } else {
                    deleteTask(selectedRaido[0].Id);
                }
            });

        $("#btn-query")
            .click(function() {
                $table.bootstrapTable('refresh');
            });
    };

    return oInit;
};

/*Operate Events*/

function createTask() {

}

function editTask(taskId) {
    abp.ajax({
        url: "/BackendTasks/edit",
            data: { "id": taskId },
            type: "GET",
            dataType: "html"
        })
        .done(function(data) {
            $("#edit").html(data);
            $("#editTask").modal("show");
        })
        .fail(function(data) {
            abp.notify.success('Edit task successfully');
        });
}

function deleteTask(taskId) {
    abp.message.confirm(
        "是否删除Id为" + taskId + "的任务信息",
        function(isConfirmed) {
            if (isConfirmed) {
                taskService.deleteTask(taskId)
                    .done(function() {
                        abp.message.success("删除成功！");
                        $table.bootstrapTable('refresh');
                    });
            }
        }
    );
}

/*End Operate Events*/


function beginPost(modalId) {
    var $modal = $(modalId);

    abp.ui.setBusy($modal);
}

function hideForm(modalId) {
    var $modal = $(modalId);

    var $form = $modal.find("form");
    abp.ui.clearBusy($modal);
    $modal.modal("hide");
    //创建成功后，要清空form表单
    $form[0].reset();
    $table.bootstrapTable('refresh');
}
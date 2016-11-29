var _taskService = abp.services.app.task;

(function ($) {

    $(function () {

        var $taskStateCombobox = $('#TaskStateCombobox');

        $taskStateCombobox.change(function () {
            location.href = '/Tasks?state=' + $taskStateCombobox.val();
        });

        var $modal = $("#add");

        var $form = $modal.find("form");

        $form.validate();

        //$form.find('button[type="submit"]').click(function (e) {            
        //    e.preventDefault();
        //    debugger;
        //    if (!$form.valid()) {
        //        return;
        //    }

        //    var creatTaskDto = $form.serializeFormToObject();
        //    abp.ui.setBusy($modal);
        //    _taskService.createTask({ creatTaskDto }).done(function () {
        //        $modal.modal("hide");

        //        location.reload(true); //reload page to see new person!
        //    }).always(function () {
        //        abp.ui.clearBusy($modal);
        //    });
        //});
        //$modal.on("shown.bs.modal", function () {
        //    $modal.find("input:not([type=hidden]):first").focus();
        //});
    });
})(jQuery);

function beginPost() {
    var $modal = $("#add");

    abp.ui.setBusy($modal);
}

function hideForm() {
    var $modal = $("#add");

    var $form = $modal.find("form");
    abp.ui.clearBusy($modal);
    $modal.modal("hide");
    $form[0].reset();
}

//function editTask(id) {
//    _taskService.getTaskById(id)
//        .done(function(data) {
//            abp.notify.success('Edit task successfully');
//            $("input[name=Name]").val(data.title);
//            $("input[name=EmailAddress]").val(data.descripation);
//            $("input[name=Id]").val(data.id);
//        });
//};

function editTask(id) {
    abp.ajax({
        url: "/tasks/edit",
        data: { "id": id },
        type: "GET",
        dataType: "html"
    })
        .done(function (data) {
            $("#modalContent").html(data);
            $("#add").modal("show");
        }).fail(function(data) {
            abp.notify.success('Edit task successfully');
        });
};

function getCreateTask() {
    abp.ajax({
            url: "/tasks/create",
            type: "GET",
            dataType: "html"
        })
        .done(function(data) {
            $("#modalContent").html(data);
            $("#add").modal("show");

        })
        .fail(function (data) {
            abp.notify.success('Edit task successfully');
        })
        .always(function () {
            
            var $modal = $("#add");
            var $form = $modal.find("form");
            var $sumbit = $form.find('button[type="submit"]');
            debugger;
            //$sumbit.live("click", createTask());
        });
}

function deleteTask(id) {
    abp.message.confirm(
        "是否删除Id为" + id + "的任务信息", function () {
            _taskService.deleteTask(id).done(function () {
                location.reload(true);
            });
        }
    );

}
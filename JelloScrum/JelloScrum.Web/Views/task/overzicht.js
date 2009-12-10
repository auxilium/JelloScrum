$(document).ready(function(){
        
    $("#addtask").click(function (event) {
        event.preventDefault();
        var taskCount = $(".taskBlock").length;
        var url = "/task/task.rails";
        $.get(url, { count: taskCount }, 
        function(data){
            $("#addtask").before(data);
        });
    });
    
     
  
});
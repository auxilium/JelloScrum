$(document).ready(function(){
    $("#progressTable tbody tr").hover(
      function () {
        $(this).addClass("hover");
      },
      function () {
        $(this).removeClass("hover");
      }
    );
    
    
    $('#progressTable .toewijzen').editable(function(value, settings) {
        element  = $(this);
        /* $.ajax({
            data: "id="+ $(this).parent().find("td:first").html() + "&value=" + value,
            url: '/sprintstory/OpslaanPrioriteiten.rails',
            success: function(html){
                element.html(html);
            },
            error: function(html){
               // location.reload();
            }
        });*/
    }, {
        loadurl : '/project/OphalenPrioriteiten.rails',
        type   : 'select',
        submit : 'save'
    });
});

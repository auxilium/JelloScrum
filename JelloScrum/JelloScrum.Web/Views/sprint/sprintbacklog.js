$(document).ready(function(){
    $("#sprintbacklog").removeClass("newButton").addClass("selectedButton");
    $.tablesorter.addParser({ 
        // set a unique id 
        id: 'haves', 
        is: function(s) { 
            // return false so this parser is not auto detected 
            return false; 
        }, 
        format: function(s) { 
            // format your data for normalization 
            return s.toLowerCase().replace(/must/,0).replace(/should/,1).replace(/could/,2).replace(/would/,3).replace(/Onbekend/,5); 
        }, 
        // set type, either numeric or text 
        type: 'numeric' 
    }); 

    $("#sprintTable").tablesorter({
        headers: {
            2: { sorter: 'haves'},
            3: { sorter: 'haves'},
            8: { sorter: false}
        },
        widgets: ['zebra']
    });

    $("#sprintTable").tablesorterPager({container: $("#pager")});
    
    $('#sprintTable .sprintprio').editable(function(value, settings) {
        element  = $(this);
        $.ajax({
            data: "id="+ $(this).parent().find("td:first").html() + "&value=" + value,
            url: '/sprintstory/OpslaanPrioriteiten.rails',
            success: function(html){
                element.html(html);
            },
            error: function(html){
               // location.reload();
            }
        });
    }, {
        loadurl : '/project/OphalenPrioriteiten.rails',
        type   : 'select',
        submit : 'save'
    });
    
    
    
});

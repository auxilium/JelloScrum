function storieClick(storie){
    currentParent = $(storie).parent().attr("id");
 
  if(currentParent == "logContainer1")
  {
    $(storie).prependTo("#logContainer2");
    
    //Nu moeten we de storie dus veranderen in een sprintstory
    sprint = $("#sprintId").val();
    story = $(storie).find(".storyId").val();
    
    dataToSend = "sprintId="+sprint+"&storyId="+story;
    
    shortInfo = $("#shortSprintInfo");     
    shortInfo.html("Loading...");
    //Hier gaan we de shortInfo vullen
    $.ajax({
      type: "POST",
      cache: false,
      url: "KoppelStoryAanSprint.rails",
      data: dataToSend,
      success: function(html){
        shortInfo.html(html);            
      }
    });     
    
    
  }else if(currentParent == "logContainer2"){  
  
    $(storie).prependTo("#logContainer1");
    
    sprint = $("#sprintId").val();
    story = $(storie).find(".storyId").val();
   
    dataToSend = "sprintId="+sprint+"&storyId="+story;
    
    shortInfo = $("#shortSprintInfo");     
    shortInfo.html("Loading...");
    //Hier gaan we de shortInfo vullen
    $.ajax({
      type: "POST",
      cache: false,
      url: "OntkoppelStoryVanSprint.rails",
      data: dataToSend,
      success: function(html){
        shortInfo.html(html);            
      }
    });     
  }
}
  
$(document).ready(function(){
    $("#logContainer1 div, #logContainer2 div").click(function(){
      storieClick($(this));
    });
    
    $("#logContainer1 div, #logContainer2 div").disableSelection();
    $("#sprintplanning").removeClass("newButton").addClass("selectedButton");
    
});  

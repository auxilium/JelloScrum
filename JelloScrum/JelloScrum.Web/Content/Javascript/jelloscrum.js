// Copyright 2009 Auxilium B.V. - http://www.auxilium.nl/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

//Initialiseer Ajax requests
jQuery.ajaxSetup({
   cache:false
});


function validate(data,form) {  
    /*
    errorContainer: "#messageBox",
    errorLabelContainer: "#messageBox ul",
    wrapper: "li", debug:true,*/
    return $(form).valid();
};

$(document).ready(function(){
    $(".megaItem .triggerKiesProject").click(function(event){
        event.preventDefault();
        $(this).parent().parent().toggleClass("active");         
    });
    
    $("#kiesProjectTable").tablesorter();
    
});

function stringReplaceDot(str)
{
try{
    return str.toString().replace(/\./g, ',');
     }
        catch(e){
         return 0;
        }
}
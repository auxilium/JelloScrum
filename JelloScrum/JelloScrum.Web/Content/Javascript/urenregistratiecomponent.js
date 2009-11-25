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

$(document).ready(function(){
    //in het menu de knop 'Uren registreren' selecteren
    $('#urenregistreren').removeClass('newButton').addClass('selectedButton');
    
    //ff inhoud selecteren van inputvakje de gebruiker er in klikt
    $('#urenregistratie :text').click(function(){
        this.select();
    });
    
    $("#urenregistratie").tablesorter({
	    headers: {
	        1: {sorter: false},
	        2: {sorter: false},
	        3: {sorter: false},
	        4: {sorter: false},
	        5: {sorter: false},
	        6: {sorter: false},
	        7: {sorter: false},
	        8: {sorter: false},
	        9: {sorter: false},
	        10: {sorter: false},
	        11: {sorter: false},
	        12: {sorter: false},
	        13: {sorter: false},
	        14: {sorter: false},
	        15: {sorter: false},
	        16: {sorter: false},
	        17: {sorter: false},
	        18: {sorter: false},
	        19: {sorter: false},
	        20: {sorter: false},
	        21: {sorter: false},
	        22: {sorter: false},
	        23: {sorter: false}
	    },
	});

});
$(document).ready(function() {
var datum1 = new Date(Date.UTC('2009','01','13','23','31','30'));
datum1 = datum1.getTime()/1000;

var datum2 = new Date(Date.UTC('2009','01','14','23','31','30'));
datum2 = datum2.getTime()/1000;
    var d5 = [[datum1,240], [datum2,200]];
   /*var uren = 240;    
    var dag = 10;
    var reductie = 240 / dag;
    
    while (dag >= 0)
    {
        uren = uren - reductie;
        d5.push([dag, uren]); 
        dag = dag - 1;
    }*/


    $.plot($("#placeholder"),
           [ {
             data: d5,
             lines: { show: true },
             points: { show: true }
             }],
             {xaxis: {
                mode: "time",
                timeformat: "%d/%m/%y"
              }}

             );
}); 

<script>
        $(document).ready(function() {
           // Lógica para fazer a ordenação por dd/MM/yyyy.
           jQuery.extend( jQuery.fn.dataTableExt.oSort, {
            "date-dia-pre": function ( a ) {
                var x;

                if ( $.trim(a) !== '' ) {
                    var frDatea = $.trim(a).split(' ');
                    var frTimea = (undefined != frDatea[1]) ? frDatea[1].split(':') : [00,00,00];
                    var frDatea2 = frDatea[0].split('/');
                    x = (frDatea2[2] + frDatea2[1] + frDatea2[0] + frTimea[0] + frTimea[1] + frTimea[2]) * 1;
                }
                else {
                    x = Infinity;
                }

                return x;
            },

            "date-dia-asc": function ( a, b ) {
                return a - b;
            },

            "date-dia-desc": function ( a, b ) {
                return b - a;
            }
        });
        
        // Lógica para fazer a ordenação por MM/yyyy
        $(document).ready(function () {
           $('#example').dataTable( {
             columnDefs: [
               { type: 'date-mes', targets: 2 }
             ]
          } );
        });

        jQuery.extend( jQuery.fn.dataTableExt.oSort, {
            "date-mes-pre": function ( a ) {
                var x;
         
                if ( $.trim(a) !== '' ) {
                    var frDatea = $.trim(a).split(' ');
                    var frDatea2 = frDatea[0].split('/');
                    x = frDatea2[1] +  frDatea2[0];
                }
                else {
                    x = Infinity;
                }
         
                return x;
            },
         
            "date-mes-asc": function ( a, b ) {
                return a - b;
            },
         
            "date-mes-desc": function ( a, b ) {
                return b - a;
            }
        } );
        });
    </script
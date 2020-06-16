#!/bin/bash

# Habilita o modo de manutenção no NGINX.
sed -i 's/\#\(.*\)\(manutencao\/index.html\)\(.*\)/\1\2\3/' /etc/nginx/sites-enabled/sinj

service nginx reload
service apache2 stop

cd /home/douglas/deploys

# Obtêm o nome do último pacote por data de criação.
LAST_DEPLOY=$(ls -t sinj_deploy_PR_* | head -1)

mv $LAST_DEPLOY ../deployed/
cd ../deployed

rm -rf ./sinj_deploy

sleep 5

tar -zxvf $LAST_DEPLOY

# Remove última versão dos componentes.
rm -rf /var/www/html/sinj*

sleep 5

cp -r sinj_deploy/* /var/www/html/

# Desabilita o modo de manutenção no NGINX.
sed -i 's/\(.*\)\(manutencao\/index.html\)\(.*\)/\#\1\2\3/' /etc/nginx/sites-enabled/sinj

service nginx reload
service apache2 start

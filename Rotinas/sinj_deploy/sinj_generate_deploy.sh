#!/bin/bash

if [ -z "$1" ] ; then
    echo "Informe o ambiente"
    exit -1
fi

#Pega o numero do versionamento
ambiente=$1
branch=$2
version="3.2.000"$(git --git-dir='/home/eduardolac/Data1/DEV_HOST/_SINJ/sinj/.git' rev-list --count $branch)

#Sinconização
rsync -rv --exclude *.cs --exclude *.designer --exclude *.csproj --exclude *.user --exclude obj/ --exclude Properties/ --exclude .git /home/eduardolac/Data1/DEV_HOST/_SINJ/sinj/Sistemas/TCDF_APL_WEB_SINJ/TCDF.Sinj.Padrao/ /home/eduardolac/Data1/DEV_HOST/_SINJ/sinj_deploy/sinj_deploy/sinjpadrao/

#Sincronização
rsync -rv --exclude *.cs --exclude *.designer --exclude *.csproj --exclude *.user --exclude obj/ --exclude Properties/ --exclude .git /home/eduardolac/Data1/DEV_HOST/_SINJ/sinj/Sistemas/TCDF_APL_WEB_SINJ/TCDF.Sinj.Portal.Web/ /home/eduardolac/Data1/DEV_HOST/_SINJ/sinj_deploy/sinj_deploy/sinj/

#Sinconização
rsync -rv --exclude *.cs --exclude *.designer --exclude *.csproj --exclude *.user --exclude obj/ --exclude Properties/ --exclude .git /home/eduardolac/Data1/DEV_HOST/_SINJ/sinj/Sistemas/TCDF_APL_WEB_SINJ/TCDF.Sinj.Web/ /home/eduardolac/Data1/DEV_HOST/_SINJ/sinj_deploy/sinj_deploy/sinjcadastro/

for i in $(find /home/eduardolac/Data1/DEV_HOST/_SINJ/sinj_deploy/sinj_deploy/ -name Web.config); do sed -i 's/<add key="versao".*\/>/<add key="versao" value="'$version'" \/>/' $i; done;

#Setando o tipo de ambiente
for i in $(find /home/eduardolac/Data1/DEV_HOST/_SINJ/sinj_deploy/sinj_deploy/ -name Web.config); do sed -i 's/<add key="Ambiente".*\/>/<add key="Ambiente" value="'$ambiente'" \/>/' $i; done;

#Gerando o arquivo compactado
find ./ -type f -name 'sinj_deploy_'$ambiente'_*.tar.gz' -exec rm {} \;
tar -zcf 'sinj_deploy_'$ambiente'_'$version'.tar.gz' sinj_deploy

exit 0


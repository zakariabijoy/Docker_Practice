#copy httpd(apache server) from dockerhub  where 'httpd' is image, ':alpine' is tag and base is alias
FROM httpd:alpine as base

#copy file from html to htdocs to run it     
COPY ./html/ /usr/local/apache2/htdocs/     
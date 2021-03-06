# Elasticsearch docker setup

## Create network

Create a newtwork
> docker network create elastic

Check
> docker network ls

## Run elasticsearch

docker run --name es01 --net elastic -p 9200:9200 -p 9300:9300 -it docker.elastic.co/elasticsearch/elasticsearch:8.2.0

Copy the generated password and enrollment token and save them in a secure location

-------------------------------------------------------------------
elastic / kJkcftK2X8Skxp_Q6Ij_
-----------------------------------------------------

https://localhost:9200/_cat/indices

## Setup memory limit
> wsl -d docker-desktop
> sysctl -w vm.max_map_count=262144

To make it persistent, you can add this line:

vm.max_map_count=262144
in your /etc/sysctl.conf and run

> sudo sysctl -p
to reload configuration with new value

## Kibana
docker run --name kib-01 --net elastic -p 5601:5601 docker.elastic.co/kibana/kibana:8.2.0

kibana-verification-code.bat


## .NET Client

Certificate fingerprint and username / password is needed
> openssl x509 -fingerprint -sha256 -in config/certs/http_ca.crt


## Developer console

Search index:

GET documents/_search
{
  "query": {
    "match_all": {}
  }
}

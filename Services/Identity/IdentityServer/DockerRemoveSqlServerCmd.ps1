docker rm $(docker stop $(docker ps --filter name=sqlServer -q))
docker volume prune -f

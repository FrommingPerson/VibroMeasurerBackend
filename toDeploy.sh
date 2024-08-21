#!/bin/zsh

# Остановка сервиса на удалённом сервере и ожидание его завершения
ssh root@80.249.149.17 "sudo systemctl stop chart.service && while pgrep -x 'chart' > /dev/null; do sleep 10; done"

# Копирование файлов на удалённый сервер
scp -rp /Users/azizmamedov/Working/WebApplication2/WebApplication2/bin/Release/net7.0/publish/* root@80.249.149.17:/var/www/chart/

# Запуск сервиса на удалённом сервере
ssh root@80.249.149.17 "sudo systemctl start chart.service"
Классы для обучения заложены в `food_classes.py`. При их изменении необходимо заново подготовить датасет для обучения и проверки.

##Подготовка датасета:
1. Очищаем `food-101/test`, `food-101/train`
2. Запускаем `split.py`

##Обучение
1. Запускаем `train.py`

##Запуск веб приложения для предсказания
1. Запускаем `app.py`
2. Пример cURL запроса(path_to_image - вставьте свой путь до файла), файлы можно добавлять продолжая список f1, f2, .. , fn:

`curl --request POST \
  --url http://localhost:14100/predict \
  --header 'Content-Type: multipart/form-data' \
  --form 'f1=*path_to_image*' \
  --form 'f2=*path_to_image*'`

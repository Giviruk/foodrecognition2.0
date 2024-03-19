import numpy as np
from keras.models import load_model
import tensorflow as tf


def predict(images_folder: str, is_plot_show=False):

    classes_to_label = {'0': 'apple_pie',
                        '1': 'beet_salad',
                        '2': 'caesar_salad',
                        '3': 'chocolate_cake',
                        '4': 'chicken_wings',
                        '5': 'french_fries',
                        '6': 'fried_rice',
                        '7': 'steak'}

    classes = list(classes_to_label.values())

    model_path = 'models/model.55-0.70.hdf5'
    model = load_model(model_path)

    # Создание датасета из директории
    dataset_dir = images_folder
    batch_size = 32
    img_height = 224
    img_width = 224

    dataset = tf.keras.utils.image_dataset_from_directory(
        dataset_dir,
        shuffle=False,
        batch_size=batch_size,
        image_size=(img_height, img_width))

    class_names = classes  # Получаем названия классов

    print(class_names)

    # Нормализация значений пикселей к [0,1]
    normalization_layer = tf.keras.layers.Rescaling(1. / 255)
    normalized_dataset = dataset.map(lambda x, y: (normalization_layer(x), y))

    # Массив для хранения названий предсказанных классов
    predicted_class_names = []

    # Предсказание и запись результатов в массив строк
    for images, _ in normalized_dataset.take(1):  # Пример с использованием только первого батча
        predictions = model.predict(images)
        predicted_classes = np.argmax(predictions, axis=1)
        predicted_class_names_batch = [class_names[i] for i in predicted_classes]
        predicted_class_names.extend(predicted_class_names_batch)

    # Вывод результатов
    print(predicted_class_names)
    return predicted_class_names

# print(predict("predict-images"))

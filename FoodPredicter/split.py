import math
import os
import shutil

import numpy as np

import food_classes

food = food_classes.classes
all_classes = os.listdir("food-101/images/")
for one_class in food:
    images = os.listdir(f"food-101/images/{one_class}/")
    np.random.shuffle(images)
    train_images = images[: math.floor(0.75 * len(images))]
    test_images = images[math.floor(0.75 * len(images)):]
    if not os.path.exists(f"food-101/train/{one_class}"):
        os.makedirs(f"food-101/train/{one_class}")
    if not os.path.exists(f"food-101/test/{one_class}"):
        os.makedirs(f"food-101/test/{one_class}")
    for img in train_images:
        shutil.copy(f"food-101/images/{one_class}/{img}", f"food-101/train/{one_class}/{img}")
    for img in test_images:
        shutil.copy(f"food-101/images/{one_class}/{img}", f"food-101/test/{one_class}/{img}")

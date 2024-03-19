import os

from PIL import Image


def crop(path, filename):
    img = Image.open(f"{path}/{filename}")
    result_folder = f"{path}/result"
    os.mkdir(result_folder)
    area1 = (310, 735, 3010, 3446)
    area2 = (3010, 300, 5000, 2000)
    area3 = (3010, 2000, 5000, 3870)
    areas = [area1, area2, area3]
    i = 1
    for area in areas:
        cropped = img.crop(area)
        cropped.save(os.path.join(result_folder, f'area{i}.jpg'))
        i = i + 1
    return result_folder

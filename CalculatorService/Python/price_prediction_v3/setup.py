import setuptools

def readme():
    with open('README.txt') as f:
        return f.read()


setuptools.setup(name='prediction',
      version='0.2',
      description='Machine learning model to predict price of plastic bags',
      long_description=readme(),
      classifiers=[
        'License :: OSI Approved :: MIT License',
        'Programming Language :: Python :: 3.7'
      ],
      keywords='Python price calculator',
      url='',
      author='Ramesh Mainali',
      author_email='ramesh.mainali2@gmail.com',
      license='MIT',
      packages=setuptools.find_packages(),
      install_requires=[
          'xgboost'
      ],
      include_package_data=True,
      package_data={'': ['model/*']})

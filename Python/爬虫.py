
from selenium import webdriver
import pymongo
import random,time
import re
#862
myclient = pymongo.MongoClient("mongodb://localhost:27017/")
mydb = myclient["essay"]

fp=webdriver.FirefoxProfile()
fp.set_preference("permissions.default.stylesheet",2)
fp.set_preference("permissions.default.image",2)
fp.set_preference("javascript.enabled",False)


driver=webdriver.Firefox(firefox_profile=fp,executable_path=r'C:\Users\Administrator\Desktop\geckodriver.exe')


# link='https://pubmed.ncbi.nlm.nih.gov/?term=lung+cancer&filter=years.2019&ac=no&format=abstract&sort=date&size=200&page='

# driver.get(link+'1')

# articles=driver.find_elements_by_class_name('results-article')

# date=articles[0].find_elements_by_class_name('cit')
# title=articles[0].find_elements_by_class_name('heading-title')
# pmid=articles[0].find_element_by_class_name('current-id')
# abstract=articles[0].find_elements_by_class_name('abstract')
# print('date:  '+date[0].text)
# print('title:  '+title[0].text)
# print('abstract:  '+abstract[0].text)
# print('pmid:  '+pmid.text)

for i in range(1,51):
    driver=webdriver.Firefox(firefox_profile=fp,executable_path=r'C:\Users\Administrator\Desktop\geckodriver.exe')

    link='https://pubmed.ncbi.nlm.nih.gov/?term=lung+cancer&filter=years.2011-2011&ac=no&format=abstract&sort=date&size=200&page='

    driver.execute_script("window.scrollTo(0,document.body.scrollHeight);")

    time.sleep(random.randint(5,15))

    driver.get(link+str(i))

    articles=driver.find_elements_by_class_name('results-article')

    print("Page:  "+str(i))

    k=0

    for article in articles:

        k=k+1
        print(k)

        date=article.find_elements_by_class_name('cit')
        title=article.find_elements_by_class_name('heading-title')
        pmid=article.find_element_by_class_name('current-id')
        abstract=article.find_elements_by_class_name('abstract')
        
        if not date:
            continue
        if not title:
            continue
        if not pmid:
            continue
        if not abstract:
            continue

        mydict={
            "date":date[0].text,
            "title":title[0].text,
            "pmid":pmid.text,
            "abstract":abstract[0].text
        }
        # year=filter(str.isdigit,date[0].text)
        #year=re.sub("\D", "", date[0].text)
        # print(year[0:4])
        # print(year)
        mycol = mydb["all"]
        mycol.insert_one(mydict)

    driver.close()
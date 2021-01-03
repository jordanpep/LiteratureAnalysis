import pymongo
import re

myclient = pymongo.MongoClient("mongodb://localhost:27017/")
mydb = myclient["essay"]
mycol = mydb["new"]

myclient1 = pymongo.MongoClient("mongodb://localhost:27017/")
mydb1 = myclient["essay"]
mycol1 = mydb["new"]

# myquery = {"month":{$exists:false}}
myquery = { "abstract": { "$regex": "Keywords:" }}
# newvalues = { "$set": { "Keywords": new1 }}

for x in mycol.find(myquery):
  id=x['pmid']
  string = x['abstract']
  p1 = re.compile(r'(Keywords)[/:](.*?)[.]', re.S) #最小匹配
  keywords=re.findall(p1, string)[0][1].strip()+';'
  # print(keywords)
  myclient1 = pymongo.MongoClient("mongodb://localhost:27017/")
  mydb1 = myclient["essay"]
  mycol1 = mydb["new"]
  print(id)
  newvalues = { "$set": { "Keywords": keywords }}
  myquery1 = { "pmid": { "$regex": id }}
  mycol1.update_one(myquery1,newvalues)


# x = mycol.update_many(myquery, newvalues)
# print(new1,x.modified_count, "文档已修改")


# x = mycol.update_many(myquery, newvalues)
#  
# print(x.modified_count, "文档已修改")


# for x in mycol.find(myquery).limit(100):
#   print(x)

# month={
#   '1':'Jan',
#   '2':'Feb',
#   '3':'Mar',
#   '4':'Apr',
#   '5':'May',
#   '6':'Jun',
#   '7':'Jul',
#   '8':'Aug',
#   '9':'Sep',
#   '10':'Oct',
#   '11':'Nov',
#   '12':'Dec'
# }

# # for i in range(2010,2022):
#   for j in range(1,13):
#     new1=str(j)
#     new2=str(j)
#     myquery = { "date": { "$regex": new2 }}
#     newvalues = { "$set": { "month": new1 } ?



"""
Created on Wed Mar 28 16:49:38 2018
@author: 47899
"""
import codecs
import os
import nltk
import math
import operator
from nltk.tokenize import WordPunctTokenizer
 
def participles(text): #分词函数
    pattern = r"""(?x)                   # set flag to allow verbose regexps 
              (?:[A-Z]\.)+           # abbreviations, e.g. U.S.A. 
              |\d+(?:\.\d+)?%?       # numbers, incl. currency and percentages 
              |\w+(?:[-']\w+)*       # words w/ optional internal hyphens/apostrophe 
              |\.\.\.                # ellipsis 
              |(?:[.,;"'?():-_`])    # special characters with meanings 
            """
    t=nltk.regexp_tokenize(text, pattern)
    length=len(t)
    for i in range(length):
        t[i]=t[i].lower() 
    return t
 
def getridofsw(lis, swlist):  # 去除文章中的停用词
    afterswlis = []
    for i in lis:
        if str(i) in swlist:
            continue
        else:
            afterswlis.append(str(i).lower())
    return afterswlis
 
def fun(filepath):  # 遍历文件夹中的所有文件，返回文件list
    arr = []
    for root, dirs, files in os.walk(filepath):
        for fn in files:
            arr.append(root+"\\"+fn)
    return arr
 
def read(path):  # 读取txt文件，并返回list
        with codecs.open(path, 'r', 'ANSI') as f:
            data=f.read()
        return data
        
def readstop(path):  # 读取txt文件，并返回list
    f = open(path,encoding='ANSI' )
    data = []
    for line in f.readlines():
        data.append(line)
    return data
        
def getstopword(path):  # 获取停用词表
    swlis = []
    for i in readstop(path):
        outsw = str(i).replace('\n', '').lower()
        swlis.append(outsw)
    return swlis
 
def freqword(wordlis):  # 统计词频，并返回字典
    freword = {}
    for i in wordlis:
        if str(i) in freword:
            count = freword[str(i)]
            freword[str(i)] = count+1
        else:
            freword[str(i)] = 1
    return freword
 
def corpus(filelist, swlist):  # 建立语料库
    alllist = []
    for i in filelist:
        afterswlis = getridofsw(participles(read(str(i))), swlist)
        alllist.append(afterswlis)
    return alllist
 
def tf_idf(wordlis, filelist, corpuslist):  # 计算TF-IDF,并返回字典
    outdic = {}
    tf = 0
    idf = 0
    dic = freqword(wordlis)
    #outlis = []
    for i in set(wordlis):
        tf = dic[str(i)]/len(wordlis)  # 计算TF：某个词在文章中出现的次数/文章总词数
        # 计算IDF：log(语料库的文档总数/(包含该词的文档数+1))
        idf = math.log(len(filelist)/(wordinfilecount(str(i), corpuslist)+1))
        tfidf = tf*idf  # 计算TF-IDF
        outdic[str(i)] = tfidf
    orderdic = sorted(outdic.items(), key=operator.itemgetter(
        1), reverse=True)  # 给字典排序
    return orderdic
 
def wordinfilecount(word, corpuslist):  # 查出包含该词的文档数
    count = 0  # 计数器
    for i in corpuslist:
        for j in i:
            if word in set(j):  # 只要文档出现该词，这计数器加1，所以这里用集合
            #if j.__contains__(word):
                count = count+1
            else:
                continue
    return count
 
def befwry(lis):  # 写入预处理，将list转为string
    outall = ''
    for i in lis:
        ech = str(i).replace("('", '').replace("',", '\t').replace(')', '')
        outall = outall+'\t'+ech+'\n'
    return outall
 
def wry(txt, path):  # 写入txt文件
    f = codecs.open(path, 'a', 'ANSI')
    f.write(txt)
    f.close()
    return path
 
def main():
    swpath = r'.\stopwords.txt'#停用词表路径
    swlist = getstopword(swpath)  # 获取停用词表列表
    #print(swlist)
    filepath = r'D:\text\yuliao\1'
    filelist = fun(filepath)
    corpuslist = corpus(filelist, swlist)
    #print(corpuslist)
    outall = ''
    wrypath = r'.\TFIDF2.txt'
    for i in filelist:
        afterswlis = getridofsw(participles(read(str(i))), swlist)  # 获取每一篇已经去除停用的词表
        tfidfdic = tf_idf(afterswlis, filelist, corpuslist) # 计算TF-IDF
        titleary = str(i).split('\\')
        title = str(titleary[-1]).replace('utf8.txt', '')
        echout = title+'\n'+befwry(tfidfdic)
        print(title+' is ok!')
        outall = outall+echout 
    print(wry(outall, wrypath)+' is ok!')
main()
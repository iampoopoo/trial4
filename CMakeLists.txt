cmake_minimum_required(VERSION 3.10)
project("Glekcraft" VERSION 0.0.0
    DESCRIPTION "A simple Minecraft clone written from scratch in C/C++ using OpenGL."
    HOMEPAGE_URL "https://github.com/glektarssza/glekcraft"
    LANGUAGES CXX)

if(CMAKE_MODULE_PATH)
    list(APPEND CMAKE_MODULE_PATH "${CMAKE_CURRENT_LIST_DIR}/cmake/modules/")
else()
    set(CMAKE_MODULE_PATH "${CMAKE_CURRENT_LIST_DIR}/cmake/modules/")
endif()

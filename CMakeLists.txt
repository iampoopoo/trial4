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

set(BUILD_SHARED_LIBS OFF CACHE BOOL "Build shared libraries" FORCE)
set(GLFW_BUILD_EXAMPLES OFF CACHE BOOL "Build the GLFW example programs" FORCE)
set(GLFW_BUILD_TESTS OFF CACHE BOOL "Build the GLFW test programs" FORCE)
set(GLFW_BUILD_DOCS OFF CACHE BOOL "Build the GLFW documentation" FORCE)
set(GLFW_INSTALL OFF CACHE BOOL "Generate installation target" FORCE)
set(GLFW_VULKAN_STATIC OFF CACHE BOOL "Assume the Vulkan loader is linked with the application" FORCE)
add_subdirectory("./deps/glfw/")

set(GLEKCRAFT_SOURCES "./src/main.cpp")
set(GLEKCRAFT_INCLUDE_DIRS "./include/")

add_executable(glekcraft ${GLEKCRAFT_SOURCES})
target_include_directories(glekcraft PRIVATE ${GLEKCRAFT_INCLUDE_DIRS})
target_compile_features(glekcraft PRIVATE cxx_std_23)
target_compile_definitions(glekcraft PRIVATE "$<$<CONFIG:Debug>:DEBUG>")
target_link_libraries(glekcraft PRIVATE glfw)

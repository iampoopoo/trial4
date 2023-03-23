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

set(SPDLOG_BUILD_ALL OFF CACHE BOOL "Build all artifacts" FORCE)
set(SPDLOG_BUILD_SHARED OFF CACHE BOOL "Build shared library" FORCE)
set(SPDLOG_ENABLE_PCH OFF CACHE BOOL "Build static or shared library using precompiled header to speed up compilation time" FORCE)
set(SPDLOG_BUILD_PIC OFF CACHE BOOL "Build position independent code (-fPIC)" FORCE)
set(SPDLOG_BUILD_EXAMPLE OFF CACHE BOOL "Build example" FORCE)
set(SPDLOG_BUILD_EXAMPLE_HO OFF CACHE BOOL "Build header only example" FORCE)
set(SPDLOG_BUILD_TESTS OFF CACHE BOOL "Build tests" FORCE)
set(SPDLOG_BUILD_TESTS_HO OFF CACHE BOOL "Build tests using the header only version" FORCE)
set(SPDLOG_BUILD_BENCH OFF CACHE BOOL "Build benchmarks (Requires https://github.com/google/benchmark.git to be installed)" FORCE)
set(SPDLOG_SANITIZE_ADDRESS OFF CACHE BOOL "Enable address sanitizer in tests" FORCE)
set(SPDLOG_BUILD_WARNINGS OFF CACHE BOOL "Enable compiler warnings" FORCE)
set(SPDLOG_INSTALL OFF CACHE BOOL "Generate the install target" FORCE)
set(SPDLOG_USE_STD_FORMAT OFF CACHE BOOL "Use std::format instead of fmt library. No compile-time format string checking." FORCE)
set(SPDLOG_FMT_EXTERNAL OFF CACHE BOOL "Use external fmt library instead of bundled" FORCE)
set(SPDLOG_FMT_EXTERNAL_HO OFF CACHE BOOL "Use external fmt header-only library instead of bundled" FORCE)
set(SPDLOG_NO_EXCEPTIONS OFF CACHE BOOL "Compile with -fno-exceptions. Call abort() on any spdlog exceptions" FORCE)

if(WIN32)
    set(SPDLOG_WCHAR_SUPPORT OFF CACHE BOOL "Support wchar api" FORCE)
    set(SPDLOG_WCHAR_FILENAMES OFF CACHE BOOL "Support wchar filenames" FORCE)
endif()

if(${CMAKE_SYSTEM_NAME} STREQUAL "Linux")
    set(SPDLOG_CLOCK_COARSE OFF CACHE BOOL "Use CLOCK_REALTIME_COARSE instead of the regular clock," FORCE)
endif()

set(SPDLOG_PREVENT_CHILD_FD OFF CACHE BOOL "Prevent from child processes to inherit log file descriptors" FORCE)
set(SPDLOG_NO_THREAD_ID OFF CACHE BOOL "prevent spdlog from querying the thread id on each log call if thread id is not needed" FORCE)
set(SPDLOG_NO_TLS OFF CACHE BOOL "prevent spdlog from using thread local storage" FORCE)
set(SPDLOG_NO_ATOMIC_LEVELS OFF CACHE BOOL "prevent spdlog from using of std::atomic log levels (use only if your code never modifies log levels concurrently" FORCE)
set(SPDLOG_DISABLE_DEFAULT_LOGGER OFF CACHE BOOL "Disable default logger creation" FORCE)
add_subdirectory("./deps/spdlog/")

add_subdirectory("./deps/glm/")

set(BUILD_UTILS OFF CACHE BOOL "utilities" FORCE)
set(GLEW_REGAL OFF CACHE BOOL "Regal mode" FORCE)
set(GLEW_OSMESA OFF CACHE BOOL "OSMesa mode" FORCE)

if(APPLE)
    set(BUILD_FRAMEWORK OFF CACHE BOOL "Build Framework bundle for OSX" FORCE)
endif()

add_subdirectory("./deps/glew/build/cmake/")

set(GLEKCRAFT_SOURCES "./src/main.cpp")
set(GLEKCRAFT_INCLUDE_DIRS "./include/")

add_executable(glekcraft ${GLEKCRAFT_SOURCES})
target_include_directories(glekcraft PRIVATE ${GLEKCRAFT_INCLUDE_DIRS})
target_compile_features(glekcraft PRIVATE cxx_std_23)
target_compile_definitions(glekcraft PRIVATE "$<$<CONFIG:Debug>:DEBUG>")
target_link_libraries(glekcraft PRIVATE glfw spdlog glm glew_s)

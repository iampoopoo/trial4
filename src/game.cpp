/**
 * @file src/game.cpp
 * @author G'lek Tarssza
 * @copyright Copyright (c) 2023 G'lek Tarssza
 * @copyright All rights reserved.
 */

#include "game.hpp"

#include <array>
#include <stdexcept>

namespace glekcraft {
    std::shared_ptr<spdlog::logger> Game::s_logger;

    Game::Game() {
        if (s_logger == nullptr) {
            s_logger = spdlog::default_logger()->clone("game");
        }
        m_initialized = false;
        m_running = false;
        m_exitCode = EXIT_SUCCESS;
        m_shouldExit = false;
        m_window = nullptr;
        m_vao = 0;
        m_vbo = 0;
        m_ebo = 0;
        m_shader = 0;
    }

    Game::~Game() {
        Terminate();
    }

    bool Game::IsInitialized() const {
        return m_initialized;
    }

    bool Game::IsRunning() const {
        return m_running;
    }

    int Game::GetExitCode() const {
        return m_exitCode;
    }

    void Game::SetExitCode(int value) {
        m_exitCode = value;
    }

    bool Game::ShouldExit() const {
        return m_shouldExit;
    }

    void Game::SetShouldExit(bool value) {
        m_shouldExit = value;
    }

    void Game::RequestExit(int exitCode) {
        SetExitCode(exitCode);
        SetShouldExit(true);
    }

    void Game::Initialize() {
        if (IsInitialized()) {
            throw std::logic_error("Game instance already initialized");
        }
        if (!glfwInit()) {
            throw std::runtime_error("Failed to initialize GLFW");
        }
        glfwWindowHint(GLFW_RESIZABLE, true);
        glfwWindowHint(GLFW_VISIBLE, false);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 5);
        glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
        glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, true);
        m_window = glfwCreateWindow(640, 480, "Glekcraft", nullptr, nullptr);
        if (m_window == nullptr) {
            throw std::runtime_error("Failed to create game window");
        }
        glfwMakeContextCurrent(m_window);
        if (glewInit() != GLEW_OK) {
            throw std::runtime_error("Failed to initialize GLEW");
        }
        glGenVertexArrays(1, &m_vao);
        if (m_vao == 0) {
            throw std::runtime_error("Failed to create vertex array object");
        }
        glGenBuffers(1, &m_vbo);
        if (m_vbo == 0) {
            throw std::runtime_error("Failed to create vertex buffer object");
        }
        glGenBuffers(1, &m_ebo);
        if (m_ebo == 0) {
            throw std::runtime_error("Failed to create element buffer object");
        }
        m_shader = glCreateProgram();
        if (m_shader == 0) {
            throw std::runtime_error("Failed to create shader program");
        }
        auto m_vShader = glCreateShader(GL_VERTEX_SHADER);
        if (m_vShader == 0) {
            throw std::runtime_error("Failed to create vertex shader");
        }
        auto m_fShader = glCreateShader(GL_FRAGMENT_SHADER);
        if (m_fShader == 0) {
            throw std::runtime_error("Failed to create fragment shader");
        }
        auto m_vShaderSource = R"(
            #version 450 core
            layout (location = 0) in vec3 vertex_position;
            void main() {
                gl_Position = vec4(vertex_position, 1.0);
            }
        )";
        auto m_fShaderSource = R"(
            #version 450 core
            out vec4 fragColor;
            void main() {
                fragColor = vec4(1.0, 0.0, 0.0, 1.0);
            }
        )";
        glShaderSource(m_vShader, 1, &m_vShaderSource, nullptr);
        glCompileShader(m_vShader);
        glShaderSource(m_fShader, 1, &m_fShaderSource, nullptr);
        glCompileShader(m_fShader);
        glAttachShader(m_shader, m_vShader);
        glAttachShader(m_shader, m_fShader);
        glLinkProgram(m_shader);
        glDeleteShader(m_vShader);
        glDeleteShader(m_fShader);
        glBindBuffer(GL_ARRAY_BUFFER, m_vbo);
        auto vertices = std::array<float, 9>{-0.5f, -0.5f, 0.0f, 0.5f, -0.5f,
                                             0.0f,  0.0f,  0.5f, 0.0f};
        glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(float),
                     vertices.data(), GL_STATIC_DRAW);
        glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, m_ebo);
        auto indices = std::array<unsigned int, 3>{0, 1, 2};
        glBufferData(GL_ELEMENT_ARRAY_BUFFER,
                     indices.size() * sizeof(unsigned int), indices.data(),
                     GL_STATIC_DRAW);
        s_logger->info("Initialized");
        m_initialized = true;
    }

    void Game::Run() {
        if (!IsInitialized()) {
            throw std::logic_error("Game instance not initialized");
        }
        if (IsRunning()) {
            throw std::logic_error("Game instance already running");
        }
        glfwShowWindow(m_window);
        m_running = true;
        while (!ShouldExit()) {
            Update();
            Render();
        }
        m_running = false;
    }

    void Game::Terminate() noexcept {
        m_initialized = false;
        if (m_shader != 0) {
            glDeleteProgram(m_shader);
            m_shader = 0;
        }
        if (m_ebo != 0) {
            glDeleteBuffers(1, &m_ebo);
            m_ebo = 0;
        }
        if (m_vbo != 0) {
            glDeleteBuffers(1, &m_vbo);
            m_vbo = 0;
        }
        if (m_vao != 0) {
            glDeleteVertexArrays(1, &m_vao);
            m_vao = 0;
        }
        if (m_window != nullptr) {
            glfwDestroyWindow(m_window);
            m_window = nullptr;
        }
        glfwTerminate();
    }

    void Game::Update() {
        glfwPollEvents();
        if (glfwWindowShouldClose(m_window)) {
            RequestExit(EXIT_SUCCESS);
            glfwSetWindowShouldClose(m_window, false);
        }
        if (glfwGetKey(m_window, GLFW_KEY_ESCAPE)) {
            RequestExit(EXIT_SUCCESS);
        }
        // TODO
    }

    void Game::Render() {
        glfwMakeContextCurrent(m_window);
        int fbWidth, fbHeight;
        glfwGetFramebufferSize(m_window, &fbWidth, &fbHeight);
        glViewport(0, 0, fbWidth, fbHeight);
        glClearColor(100.0f / 255.0f, 149.0f / 255.0f, 237.0f / 255.0f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        glBindVertexArray(m_vao);
        glUseProgram(m_shader);
        glEnableVertexAttribArray(0);
        glBindBuffer(GL_ARRAY_BUFFER, m_vbo);
        glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float),
                              nullptr);
        glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, m_ebo);
        glDrawElements(GL_TRIANGLES, 3, GL_UNSIGNED_INT, nullptr);
        glDisableVertexAttribArray(0);
        glfwSwapBuffers(m_window);
    }
} // namespace glekcraft

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
            s_logger->set_level(spdlog::level::trace);
        }
        m_initialized = false;
        m_running = false;
        m_exitCode = EXIT_SUCCESS;
        m_shouldExit = false;
        m_window = nullptr;
        m_vao = 0;
        m_vbo = nullptr;
        m_cbo = nullptr;
        m_ebo = nullptr;
        m_shader = nullptr;
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
        glBindVertexArray(m_vao);
        m_vbo = new GLBuffer(GL_ARRAY_BUFFER);
        m_cbo = new GLBuffer(GL_ARRAY_BUFFER);
        m_ebo = new GLBuffer(GL_ELEMENT_ARRAY_BUFFER);
        auto vertices = std::array<float, 9>{
            -0.5f, -0.5f, 0.0f, 0.5f, -0.5f, 0.0f, 0.0f, 0.5f, 0.0f,
        };
        m_vbo->Bind();
        glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(float),
                     vertices.data(), GL_STATIC_DRAW);
        auto colors = std::array<float, 9>{
            0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f,
        };
        m_cbo->Bind();
        glBufferData(GL_ARRAY_BUFFER, colors.size() * sizeof(float),
                     colors.data(), GL_STATIC_DRAW);
        auto indices = std::array<unsigned int, 3>{0, 1, 2};
        m_ebo->Bind();
        glBufferData(GL_ELEMENT_ARRAY_BUFFER,
                     indices.size() * sizeof(unsigned int), indices.data(),
                     GL_STATIC_DRAW);
        m_shader = new GLShaderProgram();
        auto vShader = new GLShader(GL_VERTEX_SHADER);
        auto fShader = new GLShader(GL_FRAGMENT_SHADER);
        m_shader->AttachShader(vShader);
        m_shader->AttachShader(fShader);
        vShader->UploadSource(R"(
            #version 450 core
            layout (location = 0) in vec3 aPos;
            layout (location = 1) in vec3 aColor;
            out vec3 vColor;
            void main() {
                vColor = aColor;
                gl_Position = vec4(aPos, 1.0);
            }
        )");
        fShader->UploadSource(R"(
            #version 450 core
            in vec3 vColor;
            out vec4 FragColor;
            void main() {
                FragColor = vec4(vColor, 1.0);
            }
        )");
        vShader->Compile();
        s_logger->debug(vShader->GetInfoLog());
        fShader->Compile();
        s_logger->debug(fShader->GetInfoLog());
        m_shader->Link();
        s_logger->debug(m_shader->GetInfoLog());
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
        if (m_shader != nullptr) {
            m_shader->Dispose();
            m_shader = nullptr;
        }
        if (m_ebo != nullptr) {
            m_ebo->Dispose();
            m_ebo = nullptr;
        }
        if (m_cbo != nullptr) {
            m_cbo->Dispose();
            m_cbo = nullptr;
        }
        if (m_vbo != nullptr) {
            m_vbo->Dispose();
            m_vbo = nullptr;
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
        m_shader->Activate();
        glEnableVertexAttribArray(0);
        glEnableVertexAttribArray(1);
        m_vbo->Bind();
        glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float),
                              nullptr);
        m_cbo->Bind();
        glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float),
                              nullptr);
        m_ebo->Bind();
        glDrawElements(GL_TRIANGLES, 3, GL_UNSIGNED_INT, nullptr);
        glDisableVertexAttribArray(1);
        glDisableVertexAttribArray(0);
        m_shader->Deactivate();
        glfwSwapBuffers(m_window);
    }
} // namespace glekcraft

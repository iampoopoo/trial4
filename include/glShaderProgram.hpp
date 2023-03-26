/**
 * @file include/glShaderProgram.hpp
 * @author G'lek Tarssza
 * @copyright Copyright (c) 2023 G'lek Tarssza
 * @copyright All rights reserved.
 */

#pragma once

#include <list>
#include <string>

#include <GL/glew.h>

#include "disposable.hpp"
#include "glShader.hpp"

namespace glekcraft {
    class GLShaderProgram : public Disposable {
    public:
        /**
         * Create a new instance from the specified shader sources.
         *
         * @param vertexSource The source of the vertex shader.
         * @param fragmentSource The source of the fragment shader.
         *
         * @returns A new instance.
         */
        static GLShaderProgram* CreateFromSources(const char* vertexSource,
                                                  const char* fragmentSource);

        /**
         * Create a new instance from the specified shader sources.
         *
         * @param vertexSource The source of the vertex shader.
         * @param fragmentSource The source of the fragment shader.
         *
         * @returns A new instance.
         */
        static GLShaderProgram*
        CreateFromSources(const std::string& vertexSource,
                          const std::string& fragmentSource);

    private:
        /**
         * The internal OpenGL ID of the instance.
         */
        GLuint m_id;

        /**
         * Whether the instance has been disposed.
         */
        bool m_disposed;

        /**
         * The list of shaders attached to the instance.
         */
        std::list<const GLShader*> m_shaders;

        /**
         * The copy constructor.
         *
         * @param other The other instance to copy.
         */
        GLShaderProgram(const GLShaderProgram&) = delete;

        /**
         * The copy assignment operator.
         *
         * @param other The other instance to copy.
         *
         * @returns A copy of this instance.
         */
        GLShaderProgram& operator=(const GLShaderProgram&) = delete;

    public:
        /**
         * Create a new instance.
         */
        GLShaderProgram();

        /**
         * The finalizer.
         */
        ~GLShaderProgram();

        /**
         * Get the internal OpenGL ID of the instance.
         *
         * @returns The internal OpenGL ID of the instance.
         */
        GLuint GetID() const;

        /**
         * Check whether the instance has been disposed.
         *
         * @returns Whether the instance has been disposed.
         *
         * @see Dispose
         */
        bool IsDisposed() const override;

        /**
         * Check whether the instance has been linked.
         *
         * @returns Whether the instance has been linked.
         */
        bool IsLinked() const;

        /**
         * Check whether the instance is active.
         *
         * @returns Whether the instance is active.
         */
        bool IsActive() const;

        /**
         * Check whether the instance has a shader attached.
         *
         * @param shader The shader to check for.
         *
         * @returns Whether the instance has the shader attached.
         */
        bool HasAttachedShader(const GLShader* shader) const;

        /**
         * Get the info log of the instance.
         *
         * @returns The info log of the instance.
         */
        std::string GetInfoLog() const;

        /**
         * Dispose the instance.
         *
         * @see IsDisposed
         */
        void Dispose() override;

        /**
         * Attach a shader to the instance.
         *
         * @param shader The shader to attach.
         */
        void AttachShader(const GLShader* shader);

        /**
         * Detach a shader from the instance.
         *
         * @param shader The shader to detach.
         */
        void DetachShader(const GLShader* shader);

        /**
         * Attempt to link the instance.
         */
        void Link();

        /**
         * Activate the instance.
         */
        void Activate();

        /**
         * Deactivate the instance.
         */
        void Deactivate();
    }; // class GLShaderProgram
} // namespace glekcraft

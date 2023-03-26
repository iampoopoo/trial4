/**
 * @file include/glBuffer.hpp
 * @author G'lek Tarssza
 * @copyright Copyright (c) 2023 G'lek Tarssza
 * @copyright All rights reserved.
 */

#pragma once

#include <GL/glew.h>

#include "disposable.hpp"

namespace glekcraft {
    class GLBuffer : public Disposable {
    private:
        /**
         * The internal OpenGL ID of the instance.
         */
        GLuint m_id;

        /**
         * The target the instance binds to.
         */
        GLenum m_target;

        /**
         * Whether the instance has been disposed.
         */
        bool m_disposed;

        /**
         * The copy constructor.
         *
         * @param other The other instance to copy.
         */
        GLBuffer(const GLBuffer&) = delete;

        /**
         * The copy assignment operator.
         *
         * @param other The other instance to copy.
         *
         * @returns A copy of this instance.
         */
        GLBuffer& operator=(const GLBuffer&) = delete;

    public:
        /**
         * Create a new instance.
         *
         * @param target The target the instance will bind to.
         */
        GLBuffer(GLenum target);

        /**
         * The finalizer.
         */
        ~GLBuffer();

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
         * Check whether the instance is bound to its target.
         *
         * @returns Whether the instance is bound to its target.
         */
        bool IsBound() const;

        /**
         * Dispose the instance.
         *
         * @see IsDisposed
         */
        void Dispose() override;

        /**
         * Bind the instance.
         */
        void Bind();

        /**
         * Unbind the instance.
         */
        void Unbind();
    }; // class GLBuffer
} // namespace glekcraft

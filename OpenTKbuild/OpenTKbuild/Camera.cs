﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace OpenTKbuild
{

    enum Camera_Movement
    {
        forward, 
        backward, 
        left, 
        right
    };
    
    class Camera
    {
        float Yaw = -90.0f;
        float Pitch = 0.0f;
        float Movespeed = 3.0f;
        float Sensitivity = 0.25f;
        float Zoom = 45.0f;

        Vector3 Position;
        Vector3 Front;
        Vector3 Up;
        Vector3 Right;
        Vector3 WorldUp;

        Camera(float posX, float posY, float posZ, float upX, float upY, float upZ, float yaw, float pitch) //: Front (Vector3(0.0f, 0.0f, -1.0f), Movespeed, Sensitivity, Zoom)
        {
            this.Position = new Vector3(posX, posY, posZ);
            this.WorldUp = new Vector3(upX, upY, upZ);
            this.Yaw = yaw;
            this.Pitch = pitch;
            this.updateCameraVectors();
        }

        Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(this.Position, this.Position + this.Front, this.Up);
        }

        void ProcessKeyboard(Camera_Movement direction, float deltaTime)
        {
            float velocity = this.Movespeed * deltaTime;
            if (direction == Camera_Movement.forward)
                this.Position += this.Front * velocity;
            if (direction == Camera_Movement.backward)
                this.Position -= this.Front * velocity;
            if (direction == Camera_Movement.left)
                this.Position -= this.Right * velocity;
            if (direction == Camera_Movement.right)
                this.Position += this.Right * velocity;
        }

        void ProcessMouseMovement(float xoffset, float yoffset, bool constrainPitch = true)
        {
            xoffset *= this.Sensitivity;
            yoffset *= this.Sensitivity;

            this.Yaw += xoffset;
            this.Pitch += yoffset;

            if (constrainPitch)
            {
                if (this.Pitch > 89.0f)
                    this.Pitch = 89.0f;
                if (this.Pitch < -89.0f)
                    this.Pitch = -89.0f;
            }

            this.updateCameraVectors();
        }

        void ProcessMouseScroll(float yoffset)
        {
            if (this.Zoom >= 1.0f && this.Zoom <= 45.0f)
                this.Zoom -= yoffset;
            if (this.Zoom <= 1.0f)
                this.Zoom = 1.0f;
            if (this.Zoom >= 45.0f)
                this.Zoom = 45.0f;
        }

        private void updateCameraVectors()
        {
            Vector3 front;
            front.X = (float)Math.Cos(Yaw) * (float)Math.Cos(Pitch);
            front.Y = (float)Math.Sin(Pitch);
            front.Z = (float)Math.Sin(Yaw) * (float)Math.Cos(Pitch);
            Front = front.Normalized();
            Right = Vector3.Cross(Front, WorldUp).Normalized();
            Up = Vector3.Cross(Right, Front).Normalized();

        }
    }
}

function createMovie(name, z, h)
figure

N = length(z(1,:));
for k=1:N
    clf
    
    x_k = z(1,k);
    y_k = z(3,k);
    angle_k = z(5,k);
    
    R = [cos(angle_k), -sin(angle_k); sin(angle_k), cos(angle_k)];
    
    p1 = R*[0.05; 0.05];
    p2 = R*[0.05; -0.05];
    p3 = R*[-0.05; -0.05];
    p4 = R*[-0.05; 0.05];
    
    figX = [x_k+p1(1) x_k+p2(1) x_k+p3(1) x_k+p4(1) x_k+p1(1)];
    figY = [y_k+p1(2) y_k+p2(2) y_k+p3(2) y_k+p4(2) y_k+p1(2)];
    
    plot(figX, figY, 'k', 'LineWidth', 2);
    hold on
    axis([-0.5 0.5 -0.5 0.5]);
    axis vis3d
%     drawnow
%     pause(0.01)
    movieVector(k) = getframe;
end

movieWriter = VideoWriter(name, 'MPEG-4');
movieWriter.FrameRate = 1/h;
open(movieWriter);
writeVideo(movieWriter, movieVector);
close(movieWriter);

end

